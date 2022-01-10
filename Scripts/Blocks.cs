using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace MyClasses
{

    public abstract class Block
    {
        public int ShapeIndex = -1;
        public int SpriteIndex;
        public List<List<Vector2Int>> allShapes = new List<List<Vector2Int>>();
        public abstract List<Vector2Int> ReturnNextShape();
    }

    public class TPiece : Block
    {

        readonly List<Vector2Int> shape0 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(1,0), new Vector2Int(0,1)
        };
        readonly List<Vector2Int> shape1 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(1, 0), new Vector2Int(0,-1), new Vector2Int(0,1)
        };
        readonly List<Vector2Int> shape2 = new List<Vector2Int> ()
        {
            new Vector2Int(0,0), new Vector2Int(-1, 0), new Vector2Int(1,0), new Vector2Int(0,- 1)
        };
        readonly List<Vector2Int> shape3 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(-1, 0), new Vector2Int(0,-1), new Vector2Int(0,1)
        };

        public TPiece()
        {
            allShapes.Add(shape0);
            allShapes.Add(shape1);
            allShapes.Add(shape2);
            allShapes.Add(shape3);
            SpriteIndex = 2;
        }

        public override List<Vector2Int> ReturnNextShape()
        {
            ShapeIndex++;
            if (ShapeIndex >= allShapes.Count)
            {
                ShapeIndex = -1;
                ShapeIndex++;
            }
            return allShapes[ShapeIndex];
        }
    }

    public class BPiece : Block
    {

        readonly List<Vector2Int> shape0 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(0,1)
        };

        public BPiece()
        {
            allShapes.Add(shape0);
            SpriteIndex = 2;
        }

        public override List<Vector2Int> ReturnNextShape()
        {
            ShapeIndex++;
            if (ShapeIndex >= allShapes.Count)
            {
                ShapeIndex = -1;
                ShapeIndex++;
            }
            return allShapes[ShapeIndex];
        }
    }

    public class LPieceLeft : Block
    {

        readonly List<Vector2Int> shape0 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(1,0), new Vector2Int(1,1)
        };
        readonly List<Vector2Int> shape1 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(0, 1), new Vector2Int(0,-1), new Vector2Int(1,-1)
        };
        readonly List<Vector2Int> shape2 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(1, 0), new Vector2Int(-1,0), new Vector2Int(-1,-1)
        };
        readonly List<Vector2Int> shape3 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(0, 1), new Vector2Int(0,-1), new Vector2Int(-1,1)
        };

        public LPieceLeft()
        {
            allShapes.Add(shape0);
            allShapes.Add(shape1);
            allShapes.Add(shape2);
            allShapes.Add(shape3);
            SpriteIndex = 1;
        }


        public override List<Vector2Int> ReturnNextShape()
        {
            ShapeIndex++;
            if (ShapeIndex >= allShapes.Count)
            {
                ShapeIndex = -1;
                ShapeIndex++;
            }
            return allShapes[ShapeIndex];
        }
    }
    public class LPieceRight : Block
    {

        readonly List<Vector2Int> shape0 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(1,0), new Vector2Int(-1,1)
        };
        readonly List<Vector2Int> shape1 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(0, 1), new Vector2Int(0,-1), new Vector2Int(1,1)
        };
        readonly List<Vector2Int> shape2 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(1, 0), new Vector2Int(-1,0), new Vector2Int(1,-1)
        };
        readonly List<Vector2Int> shape3 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(0, 1), new Vector2Int(0,-1), new Vector2Int(-1,-1)
        };

        public LPieceRight()
        {
            allShapes.Add(shape0);
            allShapes.Add(shape1);
            allShapes.Add(shape2);
            allShapes.Add(shape3);
            SpriteIndex = 0;
        }

        public override List<Vector2Int> ReturnNextShape()
        {
            ShapeIndex++;
            if (ShapeIndex >= allShapes.Count)
            {
                ShapeIndex = -1;
                ShapeIndex++;
            }
            return allShapes[ShapeIndex];
        }
    }
    public class ZPieceRight : Block
    {

        readonly List<Vector2Int> shape0 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(0,1), new Vector2Int(1,1)
        };
        readonly List<Vector2Int> shape1 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(0, -1), new Vector2Int(-1,0), new Vector2Int(-1,1)
        };

        public ZPieceRight()
        {
            allShapes.Add(shape0);
            allShapes.Add(shape1);
            SpriteIndex = 0;
        }
        public override List<Vector2Int> ReturnNextShape()
        {
            ShapeIndex++;
            if (ShapeIndex >= allShapes.Count)
            {
                ShapeIndex = -1;
                ShapeIndex++;
            }
            return allShapes[ShapeIndex];
        }
    }
    public class ZPieceLeft : Block
    {

        readonly List<Vector2Int> shape0 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1,1)
        };
        readonly List<Vector2Int> shape1 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(-1, 0), new Vector2Int(0,1), new Vector2Int(-1,-1)
        };

        public ZPieceLeft()
        {
            allShapes.Add(shape0);
            allShapes.Add(shape1);
            SpriteIndex = 1;
        }

        public override List<Vector2Int> ReturnNextShape()
        {
            ShapeIndex++;
            if (ShapeIndex >= allShapes.Count)
            {
                ShapeIndex = -1;
                ShapeIndex++;
            }
            return allShapes[ShapeIndex];
        }
    }
    public class IPiece : Block
    {

        readonly List<Vector2Int> shape0 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(1,0), new Vector2Int(2,0)
        };
        readonly List<Vector2Int> shape1 = new List<Vector2Int>()
        {
            new Vector2Int(0,0), new Vector2Int(0, 1), new Vector2Int(0,-1), new Vector2Int(0,-2)
        };

        public IPiece()
        {
            allShapes.Add(shape0);
            allShapes.Add(shape1);
            SpriteIndex = 2;
        }

        public override List<Vector2Int> ReturnNextShape()
        {
            ShapeIndex++;
            if (ShapeIndex >= allShapes.Count)
            {
                ShapeIndex = -1;
                ShapeIndex++;
            }
            return allShapes[ShapeIndex];
        }
    }
}
