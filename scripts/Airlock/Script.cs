
//Timings in cycles. 
const int waitForDoor = 10;
const int waitPlayerMoveMax = 100;
const int waitCooldown = 20;

const string ship_name = "myShip";

IMyInteriorLight sensor_out;
IMyInteriorLight sensor_middle;
IMyInteriorLight sensor_in;
IMyDoor door_out;
IMyDoor door_in;
long cycles;
int state;
bool s_out, s_middle, s_in;

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
    Runtime.UpdateFrequency = UpdateFrequency.Update10;
    sensor_out = GridTerminalSystem.GetBlockWithName(ship_name + "_SensorLight_Entrance_Outside") as IMyInteriorLight; 
    sensor_middle = GridTerminalSystem.GetBlockWithName(ship_name + "_SensorLight_Entrance_Middle") as IMyInteriorLight; 
    sensor_in= GridTerminalSystem.GetBlockWithName(ship_name + "_SensorLight_Entrance_Inside") as IMyInteriorLight; 
    door_out = GridTerminalSystem.GetBlockWithName(ship_name + "_Door_Entrance_Outside") as IMyDoor;  
    door_in= GridTerminalSystem.GetBlockWithName(ship_name + "_Door_Entrance_Inside") as IMyDoor;
    cycles = -1;
    state = 0;
    door_out.GetActionWithName("Open_Off").Apply(door_out); 
    door_in.GetActionWithName("Open_Off").Apply(door_in);
}

public void Save()
{
    door_out.GetActionWithName("Open_Off").Apply(door_out);  
    door_in.GetActionWithName("Open_Off").Apply(door_in);
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
    Echo("State: " + state + "; Cycles:" + cycles);
    s_out = sensor_out.Enabled;
    s_middle = sensor_middle.Enabled;
    s_in =sensor_in.Enabled;
    Echo("Out:" + s_out + ";Mid:" + s_middle + ";In:" + s_in);
    if(cycles >= 0) cycles ++;
    if(state == 0) { //Idle
        if(s_out) {
            state = 2;
            cycles = 0;
            door_out.GetActionWithName("Open_On").Apply(door_out); 
        } else if (s_in) {
            state = 1;
            cycles = 0;
            door_in.GetActionWithName("Open_On").Apply(door_in);
        }
    } else if (state == 1) { // Someone going out 1
        if(s_in) {  
            //Someone in the entrance. Do nothing or timeout.  
            if(cycles > waitPlayerMoveMax) {  
                state = 9;  
                cycles = 0; 
                door_in.GetActionWithName("Open_Off").Apply(door_in); 
            }  
        } else if (s_middle) {  
            //He has stepped in 
            state = 3;
            cycles = 0; 
            door_in.GetActionWithName("Open_Off").Apply(door_in); 
        } else {
            //He stepped out again. Close the door an go on cooldown.
            state = 8;  
            cycles = 0; 
            door_in.GetActionWithName("Open_Off").Apply(door_in);
        }
    } else if (state == 2) { // Someone going in 1
        if(s_out) {   
            //Someone in the entrance. Do nothing or timeout.   
            if(cycles > waitPlayerMoveMax) {   
                state = 10;   
                cycles = 0;  
                door_out.GetActionWithName("Open_Off").Apply(door_out);  
            }   
        } else if (s_middle) {   
            //He has stepped in  
            state = 4; 
            cycles = 0;  
            door_out.GetActionWithName("Open_Off").Apply(door_out);  
        } else { 
            //He stepped out again. Close the door an go on cooldown. 
            state = 7;   
            cycles = 0;  
            door_out.GetActionWithName("Open_Off").Apply(door_out); 
        }
    } else if (state == 3) { // Someone going out 2
        if(cycles > waitForDoor) {
            //Door closed. Need to open the other one
            state = 5;
            cycles = -1;
            door_out.GetActionWithName("Open_On").Apply(door_out);
        }
    } else if (state == 4) { // Someone going in 2
        if(cycles > waitForDoor) { 
            //Door closed. Need to open the other one 
            state = 6; 
            cycles = -1; 
            door_in.GetActionWithName("Open_On").Apply(door_in); 
        }
    } else if (state == 5) { // Someone going out 3 
        if(! s_middle) {
            //Player not in the middle anymore. Need to close the door.
            state = 7; 
            cycles = 0;
            door_out.GetActionWithName("Open_Off").Apply(door_out);
        }
    } else if (state == 6) { // Someone going in 3
        if(! s_middle) { 
            //Player not in the middle anymore. Need to close the door. 
            state = 8;  
            cycles = 0; 
            door_in.GetActionWithName("Open_Off").Apply(door_in);
        }
    } else if (state == 7) { // Someone going out 4 
        if(cycles > waitForDoor) {
            //Door colsed. Going to cooldown.
            state = 10;
            cycles = 0;
        }
    } else if (state == 8) { // Someone going in 4 
        if(cycles > waitForDoor) { 
            //Door colsed. Going to cooldown. 
            state = 9; 
            cycles = 0; 
        }
    } else if (state == 9) { // Cooldown inside
        if(s_in) {
            cycles = 0;
        }
        if(cycles > waitCooldown) { 
            state = 0; 
            cycles = -1; 
        } 
        if(s_out) { 
            state = 2;  
            cycles = 0;  
            door_out.GetActionWithName("Open_On").Apply(door_out);  
        }
    } else if (state == 10) { // Cooldown outside
        if(s_out) {
            cycles = 0;
        }
        if(cycles > waitCooldown) {
            state = 0;
            cycles = -1;
        }
        if(s_in) {
            state = 1; 
            cycles = 0; 
            door_in.GetActionWithName("Open_On").Apply(door_in); 
        }
    }
    
}

