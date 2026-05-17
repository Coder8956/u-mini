namespace UMiniFramework.Runtime.Modules
{
    public abstract class UMConfigTable
    {
        public abstract string AssetPath { get; }
        public abstract string LoadPath { get; }
        protected abstract void Init(string tableContent);
    }
}