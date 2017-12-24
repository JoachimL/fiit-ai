using Microsoft.WindowsAzure.Storage.Table;

namespace StrongR.ReadStack.Workouts.TableStorage
{
    public class Exercise : TableEntity
    {
        
        [IgnoreProperty]

        public string Id => RowKey;
        
        public string Name { get; set; }

        public string Url { get; set; }

        public string EquipmentType { get; set; }
        public string EquipmentTypeUrl { get; set; }

        public string MuscleTargeted { get; set; }
        public string Image_1 { get; set; }
    }
}
