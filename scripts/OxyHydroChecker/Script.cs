List<IMyGasTank> tanks = new List<IMyGasTank>();
List<IMyGasGenerator> oxyGens = new List<IMyGasGenerator>();
IMySoundBlock alert;
const int fillLineNum = 2;

// EDIT these: (values in percent)
const int startGenValue = 90;
const int alertValue = 30;
const string generator_name = "myShip_OxyGen";
const string sound_block_name = "myShip_SoundBlock";


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
    GridTerminalSystem.GetBlocksOfType<IMyGasTank>(tanks);
    List<IMyTerminalBlock> tmp = new List<IMyTerminalBlock>();
    GridTerminalSystem.SearchBlocksOfName(generator_name, tmp);
    for(int i = 0; i < tmp.Count; i ++) {
        oxyGens.Add((IMyGasGenerator)tmp[i]);
    }
    alert = GridTerminalSystem.GetBlockWithName(sound_block_name) as IMySoundBlock;
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
    String[] lines;
    String fillLine = "";
    String percentage = "";
    int curr;
    int min = -1;
    int max = -1;
    for( int i = 0; i < tanks.Count; i ++) {
       lines = tanks[i].DetailedInfo.Split('\n');
       fillLine = lines[fillLineNum];
       percentage = fillLine.Substring(fillLine.IndexOf(": ") + 1, fillLine.IndexOf("% ") - fillLine.IndexOf(": "));
       percentage = percentage.Substring(0, percentage.IndexOf("."));
       curr = int.Parse(percentage);
       if(min == -1 || curr < min) min = curr;
       if(max == -1 || curr > max) max = curr;
       Echo(tanks[i].CustomName + ":" + curr.ToString() + "%");
    }
    Echo("Min:" + min.ToString());
    Echo("Max:" + max.ToString());
    if(min > startGenValue) {
        for(int i = 0; i < oxyGens.Count; i ++) {
            oxyGens[i].GetActionWithName("OnOff_Off").Apply(oxyGens[i]);
        }
    } else {
        for(int i = 0; i < oxyGens.Count; i ++) { 
            oxyGens[i].GetActionWithName("OnOff_On").Apply(oxyGens[i]); 
        }
    }

    if(min < alertValue) { 
        alert.GetActionWithName("PlaySound").Apply(alert);
    } else { 
        alert.GetActionWithName("StopSound").Apply(alert);
    }
} 
