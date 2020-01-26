IMyReactor reactor;
IMyConveyorSorter sorter;
IMySoundBlock alert;

// EDIT these:
// 1 Ingot amounts to 1 000 000 here
const int refillAmount = 30 * 1000000;
const int alarmAmount = 20 *  1000000;
const String reactor_name = "myShip_reactor";
const String sorter_name = "myShip_sorter";
const String soundBlock_name = "myShip_alarm";


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
    reactor = GridTerminalSystem.GetBlockWithName(reactor_name) as IMyReactor;
    sorter = GridTerminalSystem.GetBlockWithName(sorter_name) as IMyConveyorSorter;
    alert = GridTerminalSystem.GetBlockWithName(soundBlock_name) as IMySoundBlock;
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
    long uranium = 0;
    foreach(IMyInventoryItem item in reactor.GetInventory(0).GetItems()) {
        //Reactor only holds uranium, so no type checking requiered
        uranium += item.Amount.RawValue;
    }
    Echo("Uranium: " + uranium.ToString());
    if( uranium < refillAmount) {
        sorter.GetActionWithName("OnOff_On").Apply(sorter);
    } else {
        sorter.GetActionWithName("OnOff_Off").Apply(sorter);
    }
    if( uranium < alarmAmount) { 
        alert.GetActionWithName("PlaySound").Apply(alert); 
    } else { 
        alert.GetActionWithName("StopSound").Apply(alert); 
    }
}
