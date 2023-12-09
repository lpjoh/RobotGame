﻿using System.Collections.Generic;

namespace RobotGame.Components
{
    public struct PhysicsAreaComponent
    {
        public GameRect[] Rects;
        public List<PhysicsAreaCollision> Collisions;
    }
}