namespace NexHUD.EDEngineer
{
    public class BlueprintDatas
    {
        public string Type;
        public string Name;
        public string[] Engineers;
        public BlueprintIngredient[] Ingredients;
        public BlueprintEffect[] Effects;
        public int Grade;
        public string CoriolisGuid;

        public BlueprintCategorie Categorie { get; internal set; }

        public bool IsExperimental { get { return Grade == 0; } }
    }
    public class BlueprintIngredient
    {
        public string Name;
        public int Size;
    }
    public class BlueprintEffect
    {
        public string Effect;
        public string Property;
        public bool IsGood;
    }
}
