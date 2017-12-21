using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BetterBinderBuilder {
    public partial class MainWindow : Window {
        public static string[] unitData;

        static Unit GetUnit(string name) {
            for (int i = 1; i < unitData.Count(); i += 16) {
                if (unitData[i] == name || unitData[i - 1] == name) {
                    return new Unit(unitData[i - 1],
                                    unitData[i],
                                    unitData[i + 1],
                                    unitData[i + 2],
                                    unitData[i + 3],
                                    unitData[i + 4],
                                    unitData[i + 5],
                                    unitData[i + 6],
                                    unitData[i + 7],
                                    unitData[i + 8],
                                    unitData[i + 9],
                                    unitData[i + 10],
                                    unitData[i + 11].Split(','),
                                    unitData[i + 12].Split(','),
                                    unitData[i + 13].Split(','));
                }
            }
            return new Unit();
        }

        static List<Unit> GetLevelList(string level) {
            List<Unit> temp = new List<Unit>();
            for (int i = 2; i < unitData.Count(); i += 16) {
                if (unitData[i] == level) {
                     temp.Add(new Unit(unitData[i - 2],
                                       unitData[i - 1],
                                       unitData[i],
                                       unitData[i + 1],
                                       unitData[i + 2],
                                       unitData[i + 3],
                                       unitData[i + 4],
                                       unitData[i + 5],
                                       unitData[i + 6],
                                       unitData[i + 7],
                                       unitData[i + 8],
                                       unitData[i + 9],
                                       unitData[i + 10].Split(','),
                                       unitData[i + 11].Split(','),
                                       unitData[i + 12].Split(',')));
                }
            }
            temp = temp.OrderBy(u => u.name).ToList();
            return temp;
        }

        public MainWindow() {
            InitializeComponent();

            unitData = System.IO.File.ReadAllLines("unitData.txt");

            for (int i = 1; i <= 12; i++) {
                TabItem t = new TabItem();
                t.Header = i.ToString();
                t.Height = 40;

                ListBox units = new ListBox();
                units.MouseLeftButtonUp += UnitList_MouseLeftButtonUp;
                foreach (Unit u in GetLevelList(i.ToString())) {
                    //u.PrintUnit();
                    units.Items.Add(u.name);
                }
                t.Content = units;
                Tierlist.Items.Add(t);
            }
        }

        private static Grid LoadUnitInfo(Unit host) {
            Grid Unit_Info = new Grid() { Margin = new Thickness(200, 410, 10, 15) };
            Unit_Info.Children.Add(new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(2) });

            Unit_Info.Children.Add(new Label() { Content = "Level: " + host.level });
            Unit_Info.Children.Add(new Label() { Content = host.name, Margin = new Thickness(50, 0, 0, 0) });
            Unit_Info.Children.Add(new Label() { Content = host.cost, HorizontalAlignment = HorizontalAlignment.Right });
            Unit_Info.Children.Add(new Label() { Content = "Health: " + host.health, Margin = new Thickness(0, 15, 0, 0) });

            if (host.mana != "null") Unit_Info.Children.Add(new Label() { Content = "Mana: " + host.mana, Margin = new Thickness(0, 30, 0, 0) });

            if (host.damage != "null") {
                Unit_Info.Children.Add(new Label() { Content = "Damage: " + host.damage + " " + host.damageType, Margin = new Thickness(0, 50, 0, 0) });
                Unit_Info.Children.Add(new Label() { Content = "Range: " + host.attackRange, Margin = new Thickness(0, 65, 0, 0) });
                Unit_Info.Children.Add(new Label() { Content = "Attack Speed: " + host.attackSpeed, Margin = new Thickness(0, 80, 0, 0) });
            }

            Unit_Info.Children.Add(new Label() { Content = "Armor Type: " + host.armorType, Margin = new Thickness(0, 100, 0, 0) });
            Unit_Info.Children.Add(new Label() { Content = "Move Speed: " + host.moveSpeed, Margin = new Thickness(0, 115, 0, 0) });
            
            if (host.abilities[0] != "?") {
                Unit_Info.Children.Add(new Label() { Content = "Abilites", HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 50, 0, 0) });

                for (int i = 0; i < host.abilities.Count(); i++) {
                    Unit_Info.Children.Add(new Label() { Content = host.abilities[i], HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 65 + (i * 15), 0, 0) });
                }
            }
            return Unit_Info;
        }

        private void BuildTree(Unit host, double offset, double width, int level) {
            Thickness m = new Thickness((width / 2) + offset, level * 50, 0, 0);
            Image unitImage = BuildImage(host, m);
            unitImage.MouseLeftButtonUp += UnitImage_MouseLeftButtonUp;
            Unit_Tree.Children.Add(unitImage);
            m.Top += 15;
            Unit_Tree.Children.Add(new Label() { Content = host.cost, Margin = m, FontSize = 11 });
            //if (level > 0) {
            //    tree.Children.Add(new Line() { X1 = 0, X2 = 500, Y1 = 0, Y2 = 500, Stroke = Brushes.Gray, Width = 4 });
            //}

            if (host.children[0] != "null" && level < 6) {
                for (int i = 0; i < host.children.Count(); i++) {
                    BuildTree(GetUnit(host.children[i]), offset, width / host.children.Count(), level + 1);
                    offset += width / host.children.Count();
                }
            }
        }

        private void LoadParents(Unit host) {
            if (host.parents[0] != ">") {
                for (int i = 0; i < host.parents.Count(); i++) {
                    Image temp = BuildImage(GetUnit(host.parents[i]), new Thickness(40 * i, 0, 0, 0));
                    temp.MouseLeftButtonUp += UnitImage_MouseLeftButtonUp;
                    Unit_Parents.Children.Add(temp);
                }
            }
        }

        private static Image BuildImage(Unit host, Thickness m) {
            Image hostImage = new Image();
            //hostImage.Source = new BitmapImage(new Uri(@"C:\My_Stuff\AllCode\C#\BindersData\portraits\Basic Fighter.bmp", UriKind.Absolute));
            hostImage.Source = new BitmapImage(new Uri(@"portraits\" + host.name + ".bmp", UriKind.Relative));
            hostImage.Width = hostImage.Source.Width * 0.15;
            hostImage.Height = hostImage.Source.Height * 0.15;
            hostImage.VerticalAlignment = VerticalAlignment.Top;
            hostImage.HorizontalAlignment = HorizontalAlignment.Left;
            hostImage.Margin = m;
            hostImage.Name = host.id;

            return hostImage;
        }

        private void UnitList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            ListBox tierBox = (ListBox)e.Source;
            Unit temp = new Unit();
            temp = GetUnit(tierBox.SelectedItem.ToString());

            UnitInfo_Holder.Children.Clear();
            UnitInfo_Holder.Children.Add(LoadUnitInfo(temp));
            Unit_Tree.Children.Clear();
            BuildTree(temp, 0, Unit_Tree.Width - 28.5, 0);
            Unit_Parents.Children.Clear();
            LoadParents(temp);
        }

        private void UnitImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Image treeButton = (Image)e.Source;
            Unit temp = GetUnit(treeButton.Name);

            UnitInfo_Holder.Children.Clear();
            UnitInfo_Holder.Children.Add(LoadUnitInfo(temp));
            Unit_Tree.Children.Clear();
            BuildTree(temp, 0, Unit_Tree.Width - 28.5, 0);
            Unit_Parents.Children.Clear();
            LoadParents(temp);
        }

        
    }
}
