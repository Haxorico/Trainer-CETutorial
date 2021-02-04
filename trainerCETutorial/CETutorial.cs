using System;
using System.Diagnostics;

namespace trainerCETutorial
{
  
    class CETutorial
    {
        const string GAME_NAME = "Cheat Engine Tutorial";
        const string PROCESS_NAME = "Tutorial-i386";

        const int BASE_ADDRESS_STEP2HP = 0x242650;
        readonly int[] OFFSET_STEP2HP = new int[] { 0x4AC };

        const int BASE_ADDRESS_STEP3HP = 0x242660;
        readonly int[] OFFSET_STEP3HP = new int[] { 0x4B0 };

        const int BASE_ADDRESS_STEP4HP = 0x242680;
        readonly int[] OFFSET_STEP4HP = new int[] { 0x4C0 };
        const int BASE_ADDRESS_STEP4AMMO = 0x242680;
        readonly int[] OFFSET_STEP4AMMO = new int[] { 0x4C8 };

        const int BASE_CODE_ADDRESS_STEP5 = 0x26932;

        const int BASE_ADDRESS_STEP6 = 0x2426B0;
        readonly int[] OFFSET_STEP6 = new int[] { 0x0 };

        const int BASE_CODE_ADDRESS_STEP7 = 0x275E3;

        const int BASE_ADDRESS_STEP8 = 0x2426E0;
        readonly int[] OFFSET_STEP8 = new int[] { 0x0C,0x14,0x0,0x18 };

        readonly string[] MODULE_NAME = new string[] { "Tutorial-i386.exe" };
        int[] MODULE_ADDRESSES = new int[1];
        int[] RESOLVED_ADDRESSES = new int[8];
        Process myProcess;

        
        private void solveStep2()
        {
            RESOLVED_ADDRESSES[0] = Hax.GetResolvedAddress(myProcess, MODULE_ADDRESSES[0], BASE_ADDRESS_STEP2HP, OFFSET_STEP2HP);

            //Step #1: Read value from the resolved address
            Console.WriteLine("*************************");
            Console.WriteLine("STEP2HP Before: {0}",
               Hax.GetIntFromAddress(myProcess, RESOLVED_ADDRESSES[0]));
            //Step #2: Write value to the address
            Console.WriteLine("Setting STEP2HP to 1000");
            Hax.WriteIntToAddress(myProcess, RESOLVED_ADDRESSES[0], 1000);
            //Step #3: Show the new value
            Console.WriteLine("STEP2HP After: {0}",
                Hax.GetIntFromAddress(myProcess, RESOLVED_ADDRESSES[0]));
        }
        private void solveStep3()
        {
            RESOLVED_ADDRESSES[1] = Hax.GetResolvedAddress(myProcess, MODULE_ADDRESSES[0], BASE_ADDRESS_STEP3HP, OFFSET_STEP3HP);

            //Step #1: Read value from the resolved address
            Console.WriteLine("*************************");
            Console.WriteLine("STEP3HP Before: {0}",
                Hax.GetIntFromAddress(myProcess, RESOLVED_ADDRESSES[1]));
            //Step #2: Write value to the address
            Console.WriteLine("Setting STEP3HP to 5000");
            Hax.WriteIntToAddress(myProcess, RESOLVED_ADDRESSES[1], 5000);
            //Step #3: Show the new value
            Console.WriteLine("STEP3HP After: {0}",
                Hax.GetIntFromAddress(myProcess, RESOLVED_ADDRESSES[1]));
        }
        private void solveStep4()
        {
            RESOLVED_ADDRESSES[2] = Hax.GetResolvedAddress(myProcess, MODULE_ADDRESSES[0], BASE_ADDRESS_STEP4HP, OFFSET_STEP4HP);
            RESOLVED_ADDRESSES[3] = Hax.GetResolvedAddress(myProcess, MODULE_ADDRESSES[0], BASE_ADDRESS_STEP4AMMO, OFFSET_STEP4AMMO);

            //Step #1: Read value from the resolved address
            Console.WriteLine("*************************");
            Console.WriteLine("STEP4HP Before: {0}",
                Hax.GetFloatFromAddress(myProcess, RESOLVED_ADDRESSES[2]));
            Console.WriteLine("STEP4AMMO Before: {0}",
                Hax.GetDoubleFromAddress(myProcess, RESOLVED_ADDRESSES[3]));
            //Step #2: Write value to the address
            Console.WriteLine("Setting STEP4HP to 5000");
            Hax.WriteFloatToAddress(myProcess, RESOLVED_ADDRESSES[2], 5000);
            Console.WriteLine("Setting STEP4AMMO to 5000");
            Hax.WriteDoubleToAddress(myProcess, RESOLVED_ADDRESSES[3], 5000);
            //Step #3: Show the new value
            Console.WriteLine("STEP4HP After: {0}",
                Hax.GetFloatFromAddress(myProcess, RESOLVED_ADDRESSES[2]));
            Console.WriteLine("STEP4AMMO After: {0}",
                Hax.GetDoubleFromAddress(myProcess, RESOLVED_ADDRESSES[3]));
        }
        private void solveStep5()
        {
            RESOLVED_ADDRESSES[4] = MODULE_ADDRESSES[0] + BASE_CODE_ADDRESS_STEP5;

            //Step #1: Read value from the resolved address
            Console.WriteLine("*************************");
            Console.WriteLine("Address => {0}",Hax.Dec2Hex(RESOLVED_ADDRESSES[4]));
            Console.WriteLine("STEP5 Before (BYTES): {0}",
            Hax.Bytes2Hex(Hax.GetBytesFromAddress(myProcess, RESOLVED_ADDRESSES[4], 2)));
            //Step #2: Write value to the address
            Console.WriteLine("Setting STEP5 to nops");
            Hax.NopOutAddress(myProcess, RESOLVED_ADDRESSES[4], 2);
            //Step #3: Show the new value

            Console.WriteLine("STEP5 After (BYTES): {0}",
                Hax.Bytes2Hex(Hax.GetBytesFromAddress(myProcess, RESOLVED_ADDRESSES[4], 2)));

        }
        private void solveStep6()
        {
            RESOLVED_ADDRESSES[5] = Hax.GetResolvedAddress(myProcess, MODULE_ADDRESSES[0], BASE_ADDRESS_STEP6, OFFSET_STEP6);

            //Step #1: Read value from the resolved address
            Console.WriteLine("*************************");
            Console.WriteLine("STEP6 Before: {0}",
                Hax.GetIntFromAddress(myProcess, RESOLVED_ADDRESSES[5]));
            
            //Step #2: Write value to the address
            Console.WriteLine("Setting STEP6 to 5000");
            Hax.WriteIntToAddress(myProcess, RESOLVED_ADDRESSES[5], 5000);
            //Step #3: Show the new value
            Console.WriteLine("STEP6 After: {0}",
                Hax.GetIntFromAddress(myProcess, RESOLVED_ADDRESSES[5]));
            
        }
        private void solveStep7()
        {
            RESOLVED_ADDRESSES[6] = MODULE_ADDRESSES[0] + BASE_CODE_ADDRESS_STEP7;

            //Step #1: Read value from the resolved address
            Console.WriteLine("*************************");
            Console.WriteLine("Address => {0}", Hax.Dec2Hex(RESOLVED_ADDRESSES[6]));
            Console.WriteLine("STEP7 Before (BYTES): {0}",
            Hax.Bytes2Hex(Hax.GetBytesFromAddress(myProcess, RESOLVED_ADDRESSES[6], 7)));

            //Step #2: Write value to the address
            byte[] newVal = new byte[] { 0x83, 0x83, 0xA4, 0x04, 0x00, 0x00, 0x02 };
            Console.WriteLine("Setting STEP7 to add2");
            Hax.WriteBytesToAddress(myProcess, RESOLVED_ADDRESSES[6], newVal);
            //Step #3: Show the new value

            Console.WriteLine("STEP7 Before (BYTES): {0}",
                Hax.Bytes2Hex(Hax.GetBytesFromAddress(myProcess, RESOLVED_ADDRESSES[6], 7)));

        }
        private void solveStep8()
        {
            RESOLVED_ADDRESSES[7] = Hax.GetResolvedAddress(myProcess, MODULE_ADDRESSES[0], BASE_ADDRESS_STEP8, OFFSET_STEP8);

            //Step #1: Read value from the resolved address
            Console.WriteLine("*************************");
            Console.WriteLine("STEP8 Before: {0}",
                Hax.GetIntFromAddress(myProcess, RESOLVED_ADDRESSES[7]));
            //Step #2: Write value to the address
            Console.WriteLine("Setting STEP8 to 5000");
            Hax.WriteIntToAddress(myProcess, RESOLVED_ADDRESSES[7], 5000);
            //Step #3: Show the new value
            Console.WriteLine("STEP8 After: {0}",
                Hax.GetIntFromAddress(myProcess, RESOLVED_ADDRESSES[7]));
            
        }
        
        public CETutorial()
        {
            //Step #1: Find target process
            Console.WriteLine("*************************");
            Console.WriteLine("trying to get process {0}", PROCESS_NAME);
            myProcess = Hax.GetProcessByName(PROCESS_NAME);
            if (myProcess == null)
                return;
            Console.WriteLine("myProcess.id => {0}", myProcess.Id);
            //Step #2: Resolve module base address
            Console.WriteLine("*************************");
            Console.WriteLine("Trying to get module address");
            MODULE_ADDRESSES[0] = Hax.GetModuleAddress(myProcess, MODULE_NAME[0]);
            Console.WriteLine("Module Address = {0}", Hax.Dec2Hex(MODULE_ADDRESSES[0]));
           
            while (true)
            {
                Console.Write("What step to finish: ");
                int input = int.Parse(Console.ReadLine());
                if (input == 0)
                    break;
                else if (input == 1)
                    Console.WriteLine("No need :)");
                else if (input == 2)
                    solveStep2();
                else if (input == 3)
                    solveStep3();
                else if (input == 4)
                    solveStep4();
                else if (input == 5)
                    solveStep5();
                else if (input == 6)
                    solveStep6();
                else if (input == 7)
                    solveStep7();
                else if (input == 8)
                    solveStep8();
                else
                    Console.WriteLine("NOT SUPPORTED");
            }
        }
    }
}
