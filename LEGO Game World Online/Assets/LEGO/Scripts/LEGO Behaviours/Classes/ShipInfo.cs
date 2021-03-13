using LEGOModelImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.LEGO.Behaviours;
using Unity.LEGO.Game;
using UnityEngine;

namespace Assets.LEGO.Scripts.LEGO_Behaviours
{
    class ShipInfo : MonoBehaviour
    {
        public int Hp;

        public void MinusHp()
        {
            Debug.Log("ATTACKED");

            Hp--;

            if (Hp == 0)
            {
                var brick = GetComponentInChildren<Brick>();
                if (brick)
                {
                    BrickExploder.ExplodeConnectedBricks(brick);
                }

                GameOverEvent evt = Events.GameOverEvent;
                evt.Win = false;
                EventManager.Broadcast(evt);
            }
        }
    }
}
