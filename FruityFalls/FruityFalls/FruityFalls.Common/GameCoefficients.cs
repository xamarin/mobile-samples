using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruityFalls
{
    public static class GameCoefficients
    {
        // The height of the bins - the smaller this is, the further down the fruit
        // has to go before its color is checked
        public const float BinHeight = 5;

        // The height of the splitter inbetween the bins
        public const float SplitterHeight = 60;

        // The strength of the gravity. Making this a 
        // smaller (bigger negative) number will make the
        // fruit fall faster. A larger (smaller negative) number
        // will make the fruit more floaty.
        public const float FruitGravity = -60;

        // The impact of "air drag" on the fruit, which gives
        // the fruit terminal velocity (max falling speed) and slows
        // the fruit's horizontal movement (makes the game easier)
        public const float FruitDrag = .1f;

        // Controls fruit collision bouncyness. A value of 1
        // means no momentum is lost.  A value of 0 means all
        // momentum is lost. Values greater than 1 create a spring-like effect
        public const float FruitCollisionElasticity = .5f;

        public const float FruitRadius = 16;

        public const float StartingFruitPerSecond = .1f;

        // This variable controls how many seconds must pass
        // before another fruit-per-second is added. For example, 
        // if the game initially spawns one fruit per 5 seconds, then 
        // the spawn rate is .2 fruit per second. If this value is 60, that
        // means that after 1 minute, the spawn rate will be 1.2 fruit per second.
        // Initial playtesting suggest that this value should be fairly large like 3+ 
        // minutes (180 seconds) or else the game gets hard 
        public const float TimeForExtraFruitPerSecond = 6 * 60;

        // This controls whether debug information is displayed on screen.
        public const bool ShowDebugInfo = false;

		public const bool ShowCollisionAreas = false;

        // The amount of time which must pass between bonus point awarding. 
        // Without this, the user can earn bonus points every frame
        public const float MinPointAwardingFrequency = .6f;
    }
}
