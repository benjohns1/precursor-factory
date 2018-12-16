using System;
using UnityEngine;

namespace Movement.Trajectory
{
    class Brachistochrone1D
    {
        public struct Point
        {
            public float Position;
            public float Velocity;
            public bool Complete;
            public bool Invalid;

            public Point(float position, float velocity, bool complete, bool invalid = false)
            {
                Position = position;
                Velocity = velocity;
                Complete = complete;
                Invalid = invalid;
            }
        }

        public bool IsValid { get; private set; }
        public string Info => string.Format("t0: 0, t1: {0}, t2: {1} | v0: {2}, v1: {3}, v2: {4} | r0: 0, r1: {5}, r2: {6}", t1, t2, v0, v1, v2, r1, r2);

        private readonly float initialTime;
        private readonly float initialPosition;

        private readonly float a;
        private readonly float v0;
        private readonly float t1;
        private readonly float v1;
        private readonly float r1;
        private readonly float t2;
        private readonly float v2;
        private readonly float r2;

        public Brachistochrone1D(float accelerationMagnitude, float initialTime, float initialPosition, float initialVelocity, float targetPosition, float targetVelocity = 0f)
        {
            if (Mathf.Approximately(accelerationMagnitude, 0f))
            {
                IsValid = false;
                return;
            }

            this.initialPosition = initialPosition;
            this.initialTime = initialTime;
            IsValid = true;

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
                    IsValid = false;
                    return;
                }
            }

            t1 = T1T2V1.Item1;
            t2 = T1T2V1.Item2;
            v1 = T1T2V1.Item3;
            r1 = CalculateR1(v1, t1, v0);
        }

        public Point GetPoint(float time)
        {
            if (!IsValid)
            {
                return new Point(0, 0, true, true);
            }

            float t = time - initialTime;
            float position;
            float velocity;
            bool complete = false;
            if (t < t1)
            {
                position = initialPosition + CalculateDisplacement(t, v0, a);
                velocity = CalculateVelocity(t, v0, a);
            }
            else if (t < t2)
            {
                position = initialPosition + r1 + CalculateDisplacement(t - t1, v1, -a);
                velocity = CalculateVelocity(t - t1, v1, -a);
            }
            else
            {
                position = initialPosition + r2;
                velocity = v2;
                complete = true;
            }

            return new Point(position, velocity, complete);
        }

        protected static Tuple<float, float, float, bool> calculateT1T2V1(float r2, float v0, float v2, float a)
        {
            Tuple<float, float> v1 = CalculateV1(r2, v0, v2, a);
            Tuple<float, float> t1 = new Tuple<float, float>(CalculateT1(v1.Item1, v0, a), CalculateT1(v1.Item2, v0, a));
            Tuple<float, float> t2 = new Tuple<float, float>(CalculateT2(v1.Item1, v2, a, t1.Item1), CalculateT2(v1.Item2, v2, a, t1.Item2));

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

        protected static float CalculateDisplacement(float t, float vi, float a)
        {
            return (vi * t) + (0.5f * a * t * t);
        }

        protected static float CalculateVelocity(float t, float vi, float a)
        {
            return vi + a * t;
        }

        protected static float CalculateR1(float v1, float t1, float v0)
        {
            return (0.5f * v1 * t1) + (0.5f * v0 * t1);
        }

        protected static Tuple<float, float> CalculateV1(float r2, float v0, float v2, float a)
        {
            float v1 = Mathf.Sqrt((4 * a * r2) + (2 * v0 * v0) + (2 * v2 * v2)) / 2;
            return new Tuple<float, float>(v1, -v1);
        }

        protected static float CalculateT1(float v1, float v0, float a)
        {
            return (v1 - v0) / a;
        }

        protected static float CalculateT2(float v1, float v2, float a, float t1)
        {
            return (v1 - v2 + (a * t1)) / a;
        }
    }
}
