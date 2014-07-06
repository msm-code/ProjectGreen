using OpenTK;

namespace ProjectGreen.Skeletal
{
    class Skeleton
    {
        public Bone Root { get; set; }

        public SkeletalAnimation Animation
        {
            get { return Root.AnimationOverride; }
            set 
            {
                Root.AnimationOverride = value;
                this.Animate();
            }
        }

        public Skeleton(Bone root)
        {
            this.Root = root;
        }

        public void Animate()
        {
            Root.Animate(Animation, 0);
        }

        public void Update(double delta)
        {
            Animation.Update((float)delta);
            this.Animate();
        }
    }
}
