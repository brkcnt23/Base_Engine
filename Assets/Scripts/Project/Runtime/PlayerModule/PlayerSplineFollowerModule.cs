using System;
using Base;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Runner {
    [RequireComponent(typeof(SplineFollower))]
    public class PlayerSplineFollowerModule : PlayerMobuleBase {

        #region Properties

        SplineFollower _splineFollower;
        public Action<double> OnSplineEndReached;
        [HideLabel]
        public PlayerSplineControlStats SplineControlStats;

        #endregion

        #region Module Data

        public override void Initialize(PlayerMainModule mainModule) {
            base.Initialize(mainModule);
            _splineFollower = GetComponent<SplineFollower>();
            _splineFollower.follow = false;
            _splineFollower.followMode = SplineControlStats.SplineFollowerState;
            _splineFollower.wrapMode = SplineControlStats.SplineFollowerWrap;
            _splineFollower.followSpeed = SplineControlStats.MovementSpeed;
            _splineFollower.followDuration = SplineControlStats.FixedTime;

            double2 splinePoints = new double2(0, 1);
            splinePoints.x = SplineControlStats.PositionsRange.x;
            splinePoints.y = SplineControlStats.PositionsRange.y;
            _splineFollower.SetClipRange(splinePoints.x, splinePoints.y);

            OnSplineEndReached += OnEndReached;
            _splineFollower.onEndReached += OnSplineEndReached;
        }

        public override void Activate() {
            base.Activate();
            _splineFollower.follow = true;
        }

        public override void Deactivate() {
            base.Deactivate();
        }

        protected override void OnPlayerGeneralStateChanged(PlayerGeneralState generalState) {
            base.OnPlayerGeneralStateChanged(generalState);
        }

        protected override void OnPlayerMovementStateChanged(PlayerMovementState movementState) {
            base.OnPlayerMovementStateChanged(movementState);
        }

        protected override void UnUpdate() {
            base.UnUpdate();
            MoveWithMouse();
        }

        protected override void UnFixedUpdate() {
            base.UnFixedUpdate();
        }

        protected override void UnLateUpdate() {
            base.UnLateUpdate();
        }

        public override void Uninitialized() {
            base.Uninitialized();
        }

        #endregion

        #region Module Methods

        void MoveWithMouse() {
            if (Input.GetMouseButton(0)) {
                _splineFollower.motion.offset = Vector2.Lerp(_splineFollower.motion.offset, GetMouseOffset(), Time.deltaTime * SplineControlStats.CurrentSidewaySmoothness);
                _splineFollower.motion.rotationOffset = Vector3.Lerp(_splineFollower.motion.rotationOffset, GetRotationOffset(), Time.deltaTime * SplineControlStats.CurrentRotationSmoothness);
            }
            else {
                _splineFollower.motion.rotationOffset = Vector3.Lerp(_splineFollower.motion.rotationOffset, Vector3.zero, Time.deltaTime * SplineControlStats.CurrentRotationSmoothness);
            }
        }

        Vector2 GetMouseOffset() {
            float xMove = Input.GetAxis("Mouse X") * Time.deltaTime * SplineControlStats.CurrentSidewaySpeed;
            float yMove = Input.GetAxis("Mouse Y") * Time.deltaTime * SplineControlStats.CurrentSidewaySpeed;

            SplineControlStats.MovementDelta.x += xMove;
            SplineControlStats.MovementDelta.x = Mathf.Clamp(SplineControlStats.MovementDelta.x, -SplineControlStats.MovementClamp.x, SplineControlStats.MovementClamp.x);

            SplineControlStats.MovementDelta.y += yMove;
            SplineControlStats.MovementDelta.y = Mathf.Clamp(SplineControlStats.MovementDelta.y, -SplineControlStats.MovementClamp.y, SplineControlStats.MovementClamp.y);
            return SplineControlStats.MovementDelta;
        }

        Vector3 GetRotationOffset() {
            SplineControlStats.RotationDelta.z = _splineFollower.motion.offset.x.Remap(5, -5, -SplineControlStats.RotationClamp.z, SplineControlStats.RotationClamp.z);
            SplineControlStats.RotationDelta.z = Mathf.Clamp(SplineControlStats.RotationDelta.z, -SplineControlStats.RotationClamp.z, SplineControlStats.RotationClamp.z);
            return SplineControlStats.RotationDelta;
        }

        void OnEndReached(double d) {
            _splineFollower.follow = false;
            B_GameControl.ActivateEndgame(true, 2f);
        }

        #endregion


    }

    [Serializable]
    public struct PlayerSplineControlStats {
        [Header("Controls")]
        [HideInInspector] public Vector2 MovementDelta;
        public Vector2 MovementClamp;
        [HideInInspector] public Vector3 RotationDelta;
        public Vector3 RotationClamp;
        public float CurrentSidewaySpeed;
        public float CurrentSidewaySmoothness;
        public float CurrentRotationSpeed;
        public float CurrentRotationSmoothness;
        [Header("Spline Follower")]
        public SplineFollower.FollowMode SplineFollowerState;
        public SplineFollower.Wrap SplineFollowerWrap;
        public float MovementSpeed;
        public float FixedTime;

        [MinMaxSlider(0.0f, 1.0f)]
        public Vector2 PositionsRange;

        public UnityEvent OnStart;
        public UnityEvent OnEnd;
        public UnityEvent OnUpdate;


    }
}