using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Gov.Jag.Embc.Public.ViewModels
{

    public enum Contacttypes
    {
        [EnumMember(Value = "Primary")]
        Primary = 931490000,
        [EnumMember(Value = "Additional")]
        Additional = 931490001,
        [EnumMember(Value = "BceID")]
        BceID = 931490002
    }


    public enum Equipmenttype
    {
        [EnumMember(Value = "Pill Press")]
        PillPress = 931490000,
        [EnumMember(Value = "Encapsulator")]
        Encapsulator = 931490001,
        [EnumMember(Value = "Die, Mould or Punch")]
        DieMouldorPunch = 931490003,
        [EnumMember(Value = "Pharmaceutical Mixer or Blender")]
        PharmaceuticalMixerorBlender = 931490004,
        [EnumMember(Value = "Other")]
        Other = 931490005
    }


    public enum Levelofequipmentautomation
    {
        [EnumMember(Value = "Semi-Automatic")]
        SemiAutomatic = 931490000,
        [EnumMember(Value = "Automated")]
        Automated = 931490001,
        [EnumMember(Value = "Capable of Being Automated")]
        CapableofBeingAutomated = 931490002,
        [EnumMember(Value = "Manual")]
        Manual = 931490003
    }


    public enum Pillpressencapsulatorsize
    {
        [EnumMember(Value = "Table Top Model")]
        TableTopModel = 931490000,
        [EnumMember(Value = "Free Standing Model")]
        FreeStandingModel = 931490001,
        [EnumMember(Value = "Industrial Model")]
        IndustrialModel = 931490003,
        [EnumMember(Value = "Other")]
        Other = 931490002
    }


    public enum Howwasequipmentbuilt
    {
        [EnumMember(Value = "Commercially Manufactured")]
        CommerciallyManufactured = 931490000,
        [EnumMember(Value = "Custom-built")]
        Custombuilt = 931490001,
        [EnumMember(Value = "Other")]
        Other = 931490002
    }


    public enum Productcategory
    {
        [EnumMember(Value = "Consumable")]
        Consumable = 931490000,
        [EnumMember(Value = "Non-Consumable")]
        NonConsumable = 931490001,
        [EnumMember(Value = "Other")]
        Other = 931490002
    }


    public enum Productsubcategory
    {
        [EnumMember(Value = "Food")]
        Food = 931490000,
        [EnumMember(Value = "Health Product")]
        HealthProduct = 931490001,
        [EnumMember(Value = "Candy")]
        Candy = 931490002,
        [EnumMember(Value = "Batteries")]
        Batteries = 931490003,
        [EnumMember(Value = "Cosmetics")]
        Cosmetics = 931490004,
        [EnumMember(Value = "Other")]
        Other = 931490005
    }

}