using System;
using UnityEngine;

namespace Movement
{
    struct TrajectoryPoint
    {
        public float Position;
        public float Velocity;
        public bool Complete;
        public bool Invalid;

        public TrajectoryPoint(float position, float velocity, bool complete, bool invalid = false)
        {
            Position = position;
            Velocity = velocity;
            Complete = complete;
            Invalid = invalid;
        }
    }

    class Trajectory1D
    {
        public bool InvalidTrajectory { get; protected set; }

        protected float initialTime;
        protected float initialPosition;

        protected float a;
        protected float v0;
        protected float t1;
        protected float v1;
        protected float r1;
        protected float t2;
        protected float v2;
        protected float r2;

        public Trajectory1D(float accelerationMagnitude, float initialTime, float initialPosition, float initialVelocity, float targetPosition, float targetVelocity = 0f)
        {
            //if (initialVelocity == 0 && initialPosition == targetPosition)
            this.initialPosition = initialPosition;
            this.initialTime = initialTime;

            r2 = targetPosition - initialPosition;
            v0 = initialVelocity;
            v2 = targetVelocity;
            a = accelerationMagnitude;
            Tuple<float, float, float, bool> T1T2V1 = calculateT1T2V1(r2, v0, v2, a);
            if (!T1T2V1.Item4)
            {
                // If no solutions, try reversing acceleration
                a = -a;
                T1T2V1 = calculateT1T2V1(r2, v0, v2, a);
                if (!T1T2V1.Item4)
                {
                    InvalidTrajectory = true;
                    return;
                }
            }

            t1 = T1T2V1.Item1;
            t2 = T1T2V1.Item2;
            v1 = T1T2V1.Item3;
            r1 = calculateR1(v1, t1, v0);
        }

        public string GetInfo()
        {
            return string.Format("t0: 0, t1: {0}, t2: {1} | v0: {2}, v1: {3}, v2: {4} | r0: 0, r1: {5}, r2: {6}", t1, t2, v0, v1, v2, r1, r2);
        }

        public TrajectoryPoint GetTrajectoryPoint(float time)
        {
            if (InvalidTrajectory)
            {
                return new TrajectoryPoint(0, 0, true, true);
            }

            float t = time - initialTime;
            float position;
            float velocity;
            bool complete = false;
            if (t < t1)
            {
                position = initialPosition + calculateDisplacement(t, v0, a);
                velocity = calculateVelocity(t, v0, a);
            }
            else if (t < t2)
            {
                position = initialPosition + r1 + calculateDisplacement(t - t1, v1, -a);
                velocity = calculateVelocity(t - t1, v1, -a);
            }
            else
            {
                position = initialPosition + r2;
                velocity = v2;
                complete = true;
            }

            return new TrajectoryPoint(position, velocity, complete);
        }

        protected static Tuple<float, float, float, bool> calculateT1T2V1(float r2, float v0, float v2, float a)
        {
            Tuple<float, float> v1 = calculateV1(r2, v0, v2, a);
            Tuple<float, float> t1 = new Tuple<float, float>(calculateT1(v1.Item1, v0, a), calculateT1(v1.Item2, v0, a));
            Tuple<float, float> t2 = new Tuple<float, float>(calculateT2(v1.Item1, v2, a, t1.Item1), calculateT2(v1.Item2, v2, a, t1.Item2));

            if (t1.Item1 > 0 && t2.Item1 > 0)
            {
                if (t1.Item2 > 0 && t2.Item2 > 0)
                {
                    if (t2.Item1 < t2.Item2 && t1.Item1 < t2.Item1)
                    {
                        // Both Ts are positive, first option is fastest
                        return new Tuple<float, float, float, bool>(t1.Item1, t2.Item1, v1.Item1, true);
                    }
                    else if (t1.Item2 < t2.Item2)
                    {
                        // Both Ts are positive, second option is fastest
                        return new Tuple<float, float, float, bool>(t1.Item2, t2.Item2, v1.Item2, true);
                    }
                }
                // Only first option has positive Ts
                if (t1.Item1 < t2.Item1)
                {
                    return new Tuple<float, float, float, bool>(t1.Item1, t2.Item1, v1.Item1, true);
                }
            }
            if (t1.Item2 > 0 && t2.Item2 > 0 && t1.Item2 < t2.Item2)
            {
                // Only second option has positive Ts
                return new Tuple<float, float, float, bool>(t1.Item2, t2.Item2, v1.Item2, true);
            }
            // No solution found with positive T values where t1 < t2
            return new Tuple<float, float, float, bool>(0, 0, 0, false);
        }

        protected static float calculateDisplacement(float t, float vi, float a)
        {
            return (vi * t) + (0.5f * a * t * t);
        }

        protected static float calculateVelocity(float t, float vi, float a)
        {
            return vi + a * t;
        }

        protected static float calculateR1(float v1, float t1, float v0)
        {
            return (0.5f * v1 * t1) + (0.5f * v0 * t1);
        }

        protected static Tuple<float, float> calculateV1(float r2, float v0, float v2, float a)
        {
            float v1 = Mathf.Sqrt((4 * a * r2) + (2 * v0 * v0) + (2 * v2 * v2)) / 2;
            return new Tuple<float, float>(v1, -v1);
        }

        protected static float calculateT1(float v1, float v0, float a)
        {
            return (v1 - v0) / a;
        }

        protected static float calculateT2(float v1, float v2, float a, float t1)
        {
            return (v1 - v2 + (a * t1)) / a;
        }
    }
}
