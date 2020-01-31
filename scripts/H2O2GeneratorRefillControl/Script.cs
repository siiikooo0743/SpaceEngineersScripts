// EDIT these:
const int amount_to_keep = 1000;
const String gas_generator_name = "myShip_gasGenerator";
const String ice_storage_name = "myShip_iceStorage";

// Don't edit after here unless you know what you're doing.

IMyGasGenerator gas_Generator;
IMyEntity ice_Storage;

public Program()
{
    // The constructor, called only once every session and
    // always before any other method is called. Use it to
    // initialize your script. 
    //     
    // The constructor is optional and can be removed if not
    // needed.
    // 
    // It's recommended to set RuntimeInfo.UpdateFrequency 
    // here, which will allow your script to run itself without a 
    // timer block.
    gas_Generator = GridTerminalSystem.GetBlockWithName(gas_generator_name) as IMyGasGenerator;
    ice_Storage = GridTerminalSystem.GetBlockWithName(ice_storage_name) as IMyEntity;
    Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public void Save()
{
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
    // 
    // This method is optional and can be removed if not
    // needed.
}

public void Main(string argument, UpdateType updateSource)
{
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.  

    if(gas_Generator == null) {
        Echo("Can't find gas generator!");
        return;
    }

    if(ice_Storage == null) {
        Echo("Can't find ice storage!");
        return;
    }

    MyItemType ice_type = MyItemType.MakeOre("Ice");
    MyFixedPoint targetIce = (MyFixedPoint) amount_to_keep;
    MyFixedPoint currentIce = gas_Generator.GetInventory().GetItemAmount(ice_type);
    MyFixedPoint iceToGet = targetIce - currentIce;
    Echo("ICE: " + currentIce.ToString());

    MyInventoryItem? iceItem = ice_Storage.GetInventory().FindItem(ice_type);
    if(iceToGet > 0 && iceItem.HasValue) {
        gas_Generator.GetInventory().TransferItemFrom(ice_Storage.GetInventory(), iceItem.Value, iceToGet);
    }
}
