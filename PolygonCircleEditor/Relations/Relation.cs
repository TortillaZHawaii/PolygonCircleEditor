using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Relations
{
    public abstract class Relation
    {
        public abstract void Adjust();
        public abstract void CleanUp();
    }
}
