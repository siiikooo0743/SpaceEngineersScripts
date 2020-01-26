
List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
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
    blocks = new List<IMyTerminalBlock>();
    List<IMyTerminalBlock> tmp = new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyGyro>(tmp); 
    blocks.AddRange(tmp);
    GridTerminalSystem.GetBlocksOfType<IMyThrust>(tmp);
    blocks.AddRange(tmp);
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
    if( argument != "On" && argument != "Off") {
        Echo("Need argument On or Off");
        return;
    }
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.
    for( int i = 0; i < blocks.Count; i ++) {
        blocks[i].GetActionWithName("OnOff_"+argument).Apply(blocks[i]);
    }
} 
