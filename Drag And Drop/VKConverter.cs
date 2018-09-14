using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drag_And_Drop
{
    class VKConverter
    {
           public static uint getUINT (String key)
            {
            if(key == "D0") return 0x30;
            if(key == "D1") return 0x31;
            if (key == "D2") return 0x32;
            if (key == "D3") return 0x33;
            if (key == "D4") return 0x34;
            if (key == "D5") return 0x35;
            if (key == "D6") return 0x36;
            if (key == "D7") return 0x37;
            if (key == "D8") return 0x38;
            if (key == "D9") return 0x39;
            if (key == "A") return 0x41;
            if (key == "B") return 0x42;
            if (key == "C") return 0x43;
            if (key == "D") return 0x44;
            if (key == "E") return 0x45;
            if (key == "F") return 0x46;
            if (key == "G") return 0x47;
            if (key == "H") return 0x48;
            if (key == "I") return 0x49;
            if (key == "J") return 0x4A;
            if (key == "K") return 0x4B;
            if (key == "L") return 0x4C;
            if (key == "M") return 0x4D;
            if (key == "N") return 0x4E;
            if (key == "O") return 0x4F;
            if (key == "P") return 0x50;
            if (key == "Q") return 0x51;
            if (key == "R") return 0x52;
            if (key == "S") return 0x53;
            if (key == "T") return 0x54;
            if (key == "U") return 0x55;
            if (key == "V") return 0x56;
            if (key == "W") return 0x57;
            if (key == "X") return 0x58;
            if (key == "Y") return 0x59;
            if (key == "Z") return 0x5A;
            if (key == "NUMPAD0") return 0x60;
            if (key == "NUMPAD1") return 0x61;
            if (key == "NUMPAD2") return 0x62;
            if (key == "NUMPAD3") return 0x63;
            if (key == "NUMPAD4") return 0x64;
            if (key == "NUMPAD5") return 0x65;
            if (key == "NUMPAD6") return 0x66;
            if (key == "NUMPAD7") return 0x67;
            if (key == "NUMPAD8") return 0x68;
            if (key == "NUMPAD9") return 0x69;
            if (key == "SEPARATOR") return 0x6C;
            if (key == "SUBTRACT") return 0x6D;
            if (key == "DECIMAL") return 0x6E;
            if (key == "DIVIDE") return 0x6F;
            if (key == "F1") return 0x70;
            if (key == "F2") return 0x71;
            if (key == "F3") return 0x72;
            if (key == "F4") return 0x73;
            if (key == "F5") return 0x74;
            if (key == "F6") return 0x75;
            if (key == "F7") return 0x76;
            if (key == "F8") return 0x77;
            if (key == "F9") return 0x78;
            if (key == "F10") return 0x79;
            if (key == "F11") return 0x7A;
            if (key == "F12") return 0x7B;
            if (key == "LSHIFT") return 0xA0;
            if (key == "RSHIFT") return 0xA1;
            if (key == "LCONTROL") return 0xA2;
            if (key == "RCONTROL") return 0xA3;

            if (key == "ALT") return 0x0001;
            if (key == "CTRL") return 0x0002;
            if (key == "SHIFT") return 0x0004;

            return 0x00;
            }
    }
}
