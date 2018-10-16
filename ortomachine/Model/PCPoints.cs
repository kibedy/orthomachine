namespace ortomachine.ViewModel
{
    public class PCPoints
    {
        private float x;
        private float y;
        private float z;
        private int intensity;
        private int r;
        private int g;
        private int b;

        public float X { get => x;  }
        public float Y { get => y;  }
        public float Z { get => z;  }
        public int Intensity { get => intensity;  }
        public int R { get => r;  }
        public int G { get => g;  }
        public int B { get => b; }

        public PCPoints(float x, float y, float z, int intensity, int r, int g, int b)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.intensity = intensity;
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}