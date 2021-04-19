using System.Windows.Forms;
using myseq;

namespace Structures
{
    #region GroundItem class

    public class GroundItem
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public bool isHunt { get; set; }

        public bool isCaution { get; set; }

        public bool isDanger { get; set; }

        public bool isAlert { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public ListViewItem listitem { get; set; }

        public int gone { get; set; }

        public bool Filtered { get; set; }

        public GroundItem(Spawninfo si, EQData eq)
        {
            X = si.X;
            Y = si.Y;
            Z = si.Z;
            Name = si.Name;
            Desc = GetItemDescription(si.Name, eq);
        }
        public string GetItemDescription(string ActorDef, EQData eq)
        {//sample:  IT0_ACTORDEF
            var lookupid = int.Parse(ActorDef.Remove(0, 2).Split('_')[0]);

            foreach (var item in eq.GroundSpawn)
            {
                if (item.ID.Equals(lookupid))
                {
                    return item.Name;
                }
            }
            return ActorDef;
        }
    }

    #endregion GroundItem class

    public struct ListItem
    {
        public string ActorDef { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }
}