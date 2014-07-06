namespace GreenEditor
{
    public interface IEditorCommand
    {
        string GetDescription();
        void Execute(WorldDisplay display);
    }
}
