using Oracle_Launcher.Oracle;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Oracle_Launcher.Controls
{
    /// <summary>
    /// Interaction logic for CharRealmRow.xaml
    /// </summary>
    public partial class CharRealmRow : UserControl
    {
        private enum GenderEnum {  MALE, FEMALE, UNKNOWN }

        private long Level;
        private long RaceID;
        private long ClassID;
        private long Gender;

        public CharRealmRow(string _name, long _level, long _raceID, long _classID, long _gender)
        {
            InitializeComponent();

            CharName.Text = _name;
            Level = _level;
            RaceID = _raceID;
            ClassID = _classID;
            Gender = _gender;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CharLevel.Text = Level.ToString();

            switch (RaceID)
            {
                case 1: // human
                {
                    RaceName.Text = "Human";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_human-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_human-female.png", UriKind.Relative);
                    break;
                }
                case 2: // orc
                {
                    RaceName.Text = "Orc";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_orc-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_orc-female.png", UriKind.Relative);
                    break;
                }
                case 3: // dwarf
                {
                    RaceName.Text = "Dwarf";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_dwarf-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_dwarf-female.png", UriKind.Relative);
                    break;
                }
                case 4: // night elf
                {
                    RaceName.Text = "Nightelf";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_nightelf-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_nightelf-female.png", UriKind.Relative);
                    break;
                }
                case 5: // undead
                {
                    RaceName.Text = "Undead";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_undead-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_undead-female.png", UriKind.Relative);
                    break;
                }
                case 6: // tauren
                {
                    RaceName.Text = "Tauren";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_tauren-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_tauren-female.png", UriKind.Relative);
                    break;
                }
                case 7: // gnome
                {
                    RaceName.Text = "Gnome";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_gnome-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_gnome-female.png", UriKind.Relative);
                    break;
                }
                case 8: // troll
                {
                    RaceName.Text = "Troll";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_troll-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_troll-female.png", UriKind.Relative);
                    break;
                }
                case 9: // goblin
                {
                    RaceName.Text = "Goblin";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_goblin-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_goblin-female.png", UriKind.Relative);
                    break;
                }
                case 10: // blood elf
                {
                    RaceName.Text = "Blood Elf";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_bloodelf-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_bloodelf-female.png", UriKind.Relative);
                    break;
                }
                case 11: // draenei
                {
                    RaceName.Text = "Draenei";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_draenei-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_draenei-female.png", UriKind.Relative);
                    break;
                }
                //case 12: // fel orc

                //    break;
                //case 13: // naga

                //    break;
                //case 14: // broken

                //    break;
                //case 15: // skeleton

                //    break;
                //case 16: // vrykul

                //    break;
                //case 17: // tuskarr

                //    break;
                //case 18: // forest troll

                //    break;
                //case 19: // taunka

                //    break;
                //case 20: // northrend skeleton

                //    break;
                //case 21: // ice troll

                //    break;
                case 22: // worgen
                {
                    RaceName.Text = "Worgen";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_worgen-male2.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_worgen-female2.png", UriKind.Relative);
                    break;
                }
                //case 23: // gilnean

                //    break;
                case 24: // pandaren
                case 25: // pandaren
                case 26: // pandaren
                {
                    RaceName.Text = "Pandaren";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_panda-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_panda-female.png", UriKind.Relative);
                    break;
                }
                case 27: // nightborne
                {
                    RaceName.Text = "Nightborne";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_nightborne-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_nightborne-female.png", UriKind.Relative);
                    break;
                }
                case 28: // highmountain tauren
                {
                    RaceName.Text = "Highmountain Tauren";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_highmountain-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_highmountain-female.png", UriKind.Relative);
                    break;
                }
                case 29: // void elf
                {
                    RaceName.Text = "Void Elf";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_voidelf-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_voidelf-female.png", UriKind.Relative);
                    break;
                }
                case 30: // lightforged draenei
                {
                    RaceName.Text = "Lightforged Draenei";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_lightforged-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_lightforged-female.png", UriKind.Relative);
                    break;
                }
                case 31: // zandalari troll
                {
                    RaceName.Text = "Zandalari Troll";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_ZandalariTroll-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_ZandalariTroll-female.png", UriKind.Relative);
                    break;
                }
                case 32: // kul tiran
                {
                    RaceName.Text = "Kul Tiran";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_kultiranhuman-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_kultiranhuman-female.png", UriKind.Relative);
                    break;
                }
                //case 33: // human

                //    break;
                case 34: // dark iron dwarf
                {
                    RaceName.Text = "Dark Iron Dwarf";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_darkirondwarf-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_darkirondwarf-female.png", UriKind.Relative);
                    break;
                }
                case 35: // vulpera
                {
                    RaceName.Text = "Vulpera";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/CharacterCreate-Races_vulpera-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/CharacterCreate-Races_vulpera-female.png", UriKind.Relative);
                    break;
                }
                case 36: // mag'har orc
                {
                    RaceName.Text = "Mag'har Orc";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_maghar-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_maghar-female.png", UriKind.Relative);
                    break;
                }
                case 37: // mechagnome
                {
                    RaceName.Text = "Mechagnome";
                    if (Gender == (int)GenderEnum.MALE)
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_mechagnome-male.png", UriKind.Relative);
                    else
                        ToolHandler.SetImageSource(RaceIcon, "../Assets/Race Icons/Charactercreate-races_mechagnome-female.png", UriKind.Relative);
                    break;
                }
                default:
                    break;
            }

            switch (ClassID)
            {
                case 1:
                    ClassName.Text = "Warrior";
                    break;
                case 2:
                    ClassName.Text = "Paladin";
                    break;
                case 3:
                    ClassName.Text = "Hunter";
                    break;
                case 4:
                    ClassName.Text = "Rogue";
                    break;
                case 5:
                    ClassName.Text = "Priest";
                    break;
                case 6:
                    ClassName.Text = "Death Knight";
                    break;
                case 7:
                    ClassName.Text = "Shaman";
                    break;
                case 8:
                    ClassName.Text = "Mage";
                    break;
                case 9:
                    ClassName.Text = "Warlock";
                    break;
                case 10:
                    ClassName.Text = "Monk";
                    break;
                case 11:
                    ClassName.Text = "Druid";
                    break;
                case 12:
                    ClassName.Text = "Demon Hunter";
                    break;
                default:
                    break;
            }
        }
    }
}
