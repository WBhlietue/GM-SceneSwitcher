using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.AdManager
{
    public class AdBasic : AdVideo
    {
        public static int countNum = 0;
        public int targetNum = 1;

        public override void Start()
        {
            countNum++;
            if (countNum % targetNum == 0)
            {
                base.Start();
            }
        }

        public override void Show()
        {
            if (countNum % targetNum == 0)
            {
                base.Show();
            }
        }
    }
}
