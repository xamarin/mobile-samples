using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruityFalls.Geometry
{
    public static class CollisionResponse
    {
        public static CCPoint GetSeparationVector(Circle circle, Polygon polygon)
        {
            bool isCircleCenterInPolygon = polygon.IsPointInside(
                                                   circle.PositionWorldspace.X, circle.PositionWorldspace.Y);

            float distance;
            var normal = polygon.GetNormalClosestTo(circle.PositionWorldspace, out distance);

            if (isCircleCenterInPolygon)
            {
                distance += circle.Radius;
            }
            else
            {
                distance = circle.Radius - distance;
            }

            // increase the distance by a small amount to make sure that the objects do separate:
            distance += .5f;

            var separation = normal * distance;

            return separation;
        }

        public static CCPoint ApplyBounce(CCPoint object1Velocity, CCPoint object2Velocity, CCPoint normal, float elasticity)
        {
            CCPoint vectorAsVelocity = new CCPoint(
               object1Velocity.X - object2Velocity.X,
               object1Velocity.Y - object2Velocity.Y);

            float projected = CCPoint.Dot(vectorAsVelocity, normal);

            if (projected < 0)
            {
                CCPoint velocityComponentPerpendicularToTangent =
                    normal * projected;

                object1Velocity.X -= (1 + elasticity) * velocityComponentPerpendicularToTangent.X;
                object1Velocity.Y -= (1 + elasticity) * velocityComponentPerpendicularToTangent.Y;

            }

            return object1Velocity;
        }



        private static CCPoint Reflect(CCPoint vectorToReflect, CCPoint surfaceToReflectOn)
        {
            surfaceToReflectOn.Normalize();


            CCPoint projected = surfaceToReflectOn * CCPoint.Dot(vectorToReflect, surfaceToReflectOn);

            return -(vectorToReflect - projected) + projected;

        }

    }
}
