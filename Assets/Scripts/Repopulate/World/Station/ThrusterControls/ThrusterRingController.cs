using System;
using UnityEngine;

namespace Repopulate.World.Station {
    public class ThrusterRingController : MonoBehaviour {

        [SerializeField] private ThrusterController thruster_left;
        [SerializeField] private ThrusterController thruster_up_left;
        [SerializeField] private ThrusterController thruster_up;
        [SerializeField] private ThrusterController thruster_up_right;
        [SerializeField] private ThrusterController thruster_right;
        [SerializeField] private ThrusterController thruster_down_right;
        [SerializeField] private ThrusterController thruster_down;
        [SerializeField] private ThrusterController thruster_down_left;

        [SerializeField] private bool frontRing;

        public void ManualBurn(EnumMoveDirection direction, float strength) {
            switch (direction) {
                case EnumMoveDirection.STRAFE_LEFT:
                    ManualBurn(thruster_right, strength);
                    break;
                case EnumMoveDirection.STRAFE_RIGHT:
                    ManualBurn(thruster_left, strength);
                    break;
                case EnumMoveDirection.RAISE:
                    ManualBurn(thruster_down, strength);
                    break;
                case EnumMoveDirection.LOWER:
                    ManualBurn(thruster_up, strength);
                    break;
                case EnumMoveDirection.ROTATE_YAW_POS:
                    ManualBurn(thruster_left, strength, true);
                    break;
                case EnumMoveDirection.ROTATE_YAW_NEG:
                    ManualBurn(thruster_right, strength, true);
                    break;
                case EnumMoveDirection.ROTATE_PITCH_POS:
                    ManualBurn(thruster_up, strength, true);
                    break;
                case EnumMoveDirection.ROTATE_PITCH_NEG:
                    ManualBurn(thruster_down, strength, true);
                    break;
                case EnumMoveDirection.ROTATE_ROLL_NEG:
                    ManualBurn(thruster_left, 1, strength, true);
                    ManualBurn(thruster_up_left, 1, strength, true);
                    ManualBurn(thruster_up, 1, strength, true);
                    ManualBurn(thruster_up_right, 1, strength, true);
                    ManualBurn(thruster_right, 1, strength, true);
                    ManualBurn(thruster_down_right, 1, strength, true);
                    ManualBurn(thruster_down, 1, strength, true);
                    ManualBurn(thruster_down_left, 1, strength, true);
                    break;
                case EnumMoveDirection.ROTATE_ROLL_POS:
                    ManualBurn(thruster_left, -1, strength, true);
                    ManualBurn(thruster_up_left, -1, strength, true);
                    ManualBurn(thruster_up, -1, strength, true);
                    ManualBurn(thruster_up_right, -1, strength, true);
                    ManualBurn(thruster_right, -1, strength, true);
                    ManualBurn(thruster_down_right, -1, strength, true);
                    ManualBurn(thruster_down, -1, strength, true);
                    ManualBurn(thruster_down_left, -1, strength, true);
                    break;
            }
        }

        public void ManualBurnVector(Vector2 burnVector) {
            float x = burnVector.x;
            float y = burnVector.y;

            if (x < 0 && y < 0) {
                ManualBurn(thruster_down_right, 0, x);
            }

            if (x > 0 && y < 0) {
                ManualBurn(thruster_down_left, 0, x);
            }

            if (x < 0 && y > 0) {
                ManualBurn(thruster_up_right, 0, x);
            }

            if (x > 0 && y > 0) {
                ManualBurn(thruster_up_left, 0, x);
            }

            if (x < 0) {
                ManualBurn(thruster_right, 0, x);
            }

            if (x > 0) {
                ManualBurn(thruster_left, 0, x);
            }

            if (y < 0) {
                ManualBurn(thruster_down, 0, y);
            }

            if (y > 0) {
                ManualBurn(thruster_up, 0, y);
            }
        }

        public void ScheduleBurn(EnumMoveDirection direction, float timeSecs) => ScheduleBurn(direction, 0, timeSecs, 1);
        public void ScheduleBurn(EnumMoveDirection direction, float timeSecs, float strength) => ScheduleBurn(direction, 0, timeSecs, strength);

        public void ScheduleBurn(EnumMoveDirection direction, float angle, float timeSecs, float strength) {
            switch (direction) {
                case EnumMoveDirection.STRAFE_LEFT:
                    AddBurn(thruster_right, angle, timeSecs, strength);
                    break;
                case EnumMoveDirection.STRAFE_RIGHT:
                    AddBurn(thruster_left, angle, timeSecs, strength);
                    break;
                case EnumMoveDirection.RAISE:
                    AddBurn(thruster_down, angle, timeSecs, strength);
                    break;
                case EnumMoveDirection.LOWER:
                    AddBurn(thruster_up, angle, timeSecs, strength);
                    break;
                case EnumMoveDirection.ROTATE_YAW_POS:
                    AddBurn(thruster_left, angle, timeSecs, strength, true);
                    break;
                case EnumMoveDirection.ROTATE_YAW_NEG:
                    AddBurn(thruster_right, angle, timeSecs, strength, true);
                    break;
                case EnumMoveDirection.ROTATE_PITCH_POS:
                    AddBurn(thruster_up, angle, timeSecs, strength, true);
                    break;
                case EnumMoveDirection.ROTATE_PITCH_NEG:
                    AddBurn(thruster_down, angle, timeSecs, strength, true);
                    break;
                case EnumMoveDirection.ROTATE_ROLL_NEG:
                    AddBurn(thruster_left, 1, timeSecs, strength, true);
                    AddBurn(thruster_up_left, 1, timeSecs, strength, true);
                    AddBurn(thruster_up, 1, timeSecs, strength, true);
                    AddBurn(thruster_up_right, 1, timeSecs, strength, true);
                    AddBurn(thruster_right, 1, timeSecs, strength, true);
                    AddBurn(thruster_down_right, 1, timeSecs, strength, true);
                    AddBurn(thruster_down, 1, timeSecs, strength, true);
                    AddBurn(thruster_down_left, 1, timeSecs, strength, true);
                    break;
                case EnumMoveDirection.ROTATE_ROLL_POS:
                    AddBurn(thruster_left, -1, timeSecs, strength, true);
                    AddBurn(thruster_up_left, -1, timeSecs, strength, true);
                    AddBurn(thruster_up, -1, timeSecs, strength, true);
                    AddBurn(thruster_up_right, -1, timeSecs, strength, true);
                    AddBurn(thruster_right, -1, timeSecs, strength, true);
                    AddBurn(thruster_down_right, -1, timeSecs, strength, true);
                    AddBurn(thruster_down, -1, timeSecs, strength, true);
                    AddBurn(thruster_down_left, -1, timeSecs, strength, true);
                    break;
                case EnumMoveDirection.NONE:
                    AddBurn(thruster_left, 0, timeSecs, 0, true);
                    AddBurn(thruster_up_left, 0, timeSecs, 0, true);
                    AddBurn(thruster_up, 0, timeSecs, 0, true);
                    AddBurn(thruster_up_right, 0, timeSecs, 0, true);
                    AddBurn(thruster_right, 0, timeSecs, 0, true);
                    AddBurn(thruster_down_right, 0, timeSecs, 0, true);
                    AddBurn(thruster_down, 0, timeSecs, 0, true);
                    AddBurn(thruster_down_left, 0, timeSecs, 0, true);
                    break;
            }
        }

        private void ManualBurn(ThrusterController thruster, float strength, bool rearOpposite = false) {
            ManualBurn(thruster, 0, strength, rearOpposite);
        }

        private void ManualBurn(ThrusterController thruster, float angle, float strength, bool rearOpposite = false) {
            strength = Math.Abs(strength);
            if (!frontRing && rearOpposite) {
                thruster.Opposite().Burn(angle, strength);
            }
            else {
                thruster.Burn(angle, strength);
            }
        }

        private void AddBurn(ThrusterController thruster, float angle, float time, float strength, bool rearOpposite = false) {
            if (!frontRing && rearOpposite) {
                thruster.Opposite().AddBurn(angle, time, strength);
            }
            else {
                thruster.AddBurn(angle, time, strength);
            }
        }
    }
}