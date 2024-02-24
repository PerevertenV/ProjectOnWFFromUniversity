using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;

namespace Kursova_
{
    public partial class SMF : Form, ITakePath
    {
        public SMF()
        {
            InitializeComponent();
            panel3.Hide();
            MethodForPhotos("D:\\НУОП\\2\\1 семестр\\Курсові\\ООП\\1.jpg", FirstPB, 20);
            MethodForPhotos("D:\\НУОП\\2\\1 семестр\\Курсові\\ООП\\6.jpg", SecPB, 360);
            MethodForPhotos("D:\\НУОП\\2\\1 семестр\\Курсові\\ООП\\7.jpg", ThirdPB, 700);
            MethodForPhotos("D:\\НУОП\\2\\1 семестр\\Курсові\\ООП\\9.jpg", FourthPB, 1040);
        }
        /////////////// код для надання можливості руху форми
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        ///////////////
        public List<Customer> CustomerList = new List<Customer>();
        public List<Order> OrderList = new List<Order>();
        public List<Agronomist> AgronomistList = new List<Agronomist>();
        // методи для створення та додавання об'єктів класів
        public void methodForAddingCustomer(string FN, string PN, string Adr, string ID, string AgronomistFN)
        {
            Customer newCust = new Customer(FN, PN, Adr, ID, AgronomistFN);
            CustomerList.Add(newCust);
        }
        public void methodForAddingOrder(string Adr, string TOP, string DS, string DE, bool TOS,
            double Siz, double prc, string IDO, string IDC)
        {
            Order newOr = new Order(Adr, TOP, DS, DE, Siz, prc, IDO, IDC, TOS);
            OrderList.Add(newOr);
        }
        public void methodForAddingAgronomist(string FN, string PN)
        {
            Agronomist newAgr = new Agronomist(FN, PN);
            AgronomistList.Add(newAgr);
        }
        //подія(запуск форми)
        async private void Form1_Load(object sender, EventArgs e)
        {

            if (!File.Exists(TakePath() + "/CustomerFile.txt"))
            {
                methodForAddingCustomer("Гальченко Іван Степанович", "0944563449", "вул.Сегедська 43", "000001",
                    "Стеценко Олег Михайлович");
                //CustomerFN Adr CustomerPN ID 

                int index = CustomerList.Count - 1;
                string info = CustomerList[index].getPeopleFullName()
                    + "/" + CustomerList[index].getAdress()
                    + "/" + CustomerList[index].getPeoplePhoneNumber()
                    + "/" + CustomerList[index].getCustomerID()
                    + "/" + CustomerList[index].getAgronomisFN();

                WriteToFile wt = new WriteToFile(info, 3);

            }
            if (!File.Exists(TakePath() + "/OrderFile.txt"))
            {
                methodForAddingOrder("вул.Сегедська 43", "Дерева", "01.10.2023", "10.10.2023",
                true, 430, 45600, "000001", CustomerList[0].getCustomerID());

                //TOP DS DE TOS size Adr price IDO IDC 

                int index = OrderList.Count - 1;
                string info = OrderList[index].typeOfPlant
                    + "/" + OrderList[index].DateOfStart
                    + "/" + OrderList[index].DateOfEnd
                    + "/" + OrderList[index].TypeOfService.ToString()
                    + "/" + OrderList[index].Size.ToString()
                    + "/" + OrderList[index].price.ToString()
                    + "/" + OrderList[index].getAdress()
                    + "/" + OrderList[index].IDOfOrder
                    + "/" + OrderList[index].IDofCustomerThatMadeThisOrder;
                WriteToFile wt = new WriteToFile(info, 1);
            }
            if (!File.Exists(TakePath() + "/AgronomistFile.txt"))
            {

                methodForAddingAgronomist("Стеценко Олег Михайлович", "0955664448");
                int index = AgronomistList.Count - 1;
                string info = AgronomistList[index].getPeopleFullName()
                    + "/" + AgronomistList[index].getPeoplePhoneNumber()
                    + "/" + Agronomist.getBrunchName();

                WriteToFile wt = new WriteToFile(info, 2);
                CustomerList.RemoveAt(CustomerList.Count - 1);
                AgronomistList.RemoveAt(AgronomistList.Count - 1);
                OrderList.RemoveAt(OrderList.Count - 1);
            }

            methodForReadingFromFileOrder();
            methodForReadingFromFileAgronomist();
            methodForReadingFromFileCustomer();


            PrintWithTypewriterEffect(SecondActiveText, "Smart Agro - Smart Future - Smart Life", 70);
            await Task.Delay(3939);
            PrintWithTypewriterEffect(ThirdActiveText, "Переверни свій коцепт сприйняття сфери агро", 20);

        }
        //методи для зчитування із файлів
        public void methodForReadingFromFileOrder()
        {
            string ResultFromFile1 = File.ReadAllText(TakePath() + "/OrderFile.txt");

            string Adr, TOP, DS, DE, IDO, IDC;
            string[] FirstArr, SecArr;
            double size, price;
            bool TOS;

            FirstArr = ResultFromFile1.Split('\n');
            for (int i = 0; i < FirstArr.Length - 1; i++)
            {
                SecArr = FirstArr[i].Split('/');

                TOP = SecArr[0].ToString();
                DS = SecArr[1].ToString();
                DE = SecArr[2].ToString();
                TOS = bool.Parse(SecArr[3]);
                size = double.Parse(SecArr[4]);
                Adr = SecArr[6].ToString();
                price = double.Parse(SecArr[5]);
                IDO = SecArr[8].ToString();
                IDC = SecArr[7].ToString();

                methodForAddingOrder(Adr, TOP, DS, DE, TOS, price, size, IDO, IDC);

                Array.Clear(SecArr, 0, SecArr.Length);
            }
        }
        //метод для отримання шляху із реєстру
        public string TakePath()
        {
            string path = "";
            using (RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"Software\КурсоваРобота"))
            {
                if (reg != null)
                {
                    path = reg.GetValue("path").ToString();
                }
                else
                {
                    Registry.CurrentUser.CreateSubKey(@"Software\КурсоваРобота").SetValue("path", "D:");
                }
            }
            return path;
        }
        //методи для зчитування із файлів
        public void methodForReadingFromFileCustomer()
        {
            string ResultFromFile1 = File.ReadAllText(TakePath() + "/CustomerFile.txt");

            string Adr, CustomerFN, CustomerPN, ID, AgronomistFN;
            string[] FirstArr, SecArr;


            FirstArr = ResultFromFile1.Split('\n');
            for (int i = 0; i < FirstArr.Length - 1; i++)
            {
                SecArr = FirstArr[i].Split('/');

                CustomerFN = SecArr[0].ToString();
                Adr = SecArr[1].ToString();
                CustomerPN = SecArr[2].ToString();
                ID = SecArr[3].ToString();
                AgronomistFN = SecArr[4].ToString();

                methodForAddingCustomer(CustomerFN, CustomerPN, Adr, ID, AgronomistFN);

                Array.Clear(SecArr, 0, SecArr.Length);
            }
        }

        public void methodForReadingFromFileAgronomist()
        {
            string ResultFromFile1 = File.ReadAllText(TakePath() + "/AgronomistFile.txt");

            string AgronomistFullName, AgronomistPN;
            string[] FirstArr, SecArr;

            FirstArr = ResultFromFile1.Split('\n');
            for (int i = 0; i < FirstArr.Length - 1; i++)
            {
                SecArr = FirstArr[i].Split('/');
                AgronomistFullName = SecArr[0].ToString();
                AgronomistPN = SecArr[1].ToString();
                methodForAddingAgronomist(AgronomistFullName, AgronomistPN);
                Array.Clear(SecArr, 0, SecArr.Length);
            }
        }
        private void MoveUse(object sender, MouseEventArgs e) // надаємо можливість формі рухатись
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        async private void CloseButton(object sender, EventArgs e) // для плавного закриття
        {
            for (; this.Opacity > 0; this.Opacity -= 0.2) { await Task.Delay(40); } // затримка
            this.Close();
        }
        private void HideButton(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;// згортає програму
        }
        //подія(обираємо який об'єкт створювати)
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Choise.SelectedIndex == 0)
            {
                AgronomistGB.Hide();
                OrderGB.Hide();
                CustomerBox.Location = new Point(10, 33);
                CustomerBox.Show();
                CustomerBox.Height = 533;
            }
            else if (Choise.SelectedIndex == 1)
            {
                CustomerBox.Hide();
                OrderGB.Hide();
                AgronomistGB.Location = new Point(10, 33);
                AgronomistGB.Show();
                AgronomistGB.Height = 433;
            }
            else if (Choise.SelectedIndex == 2)
            {
                CustomerBox.Hide();
                AgronomistGB.Hide();
                OrderGB.Location = new Point(10, 33);
                OrderGB.Show();
                OrderGB.Height = 537;

                List<string> AtribList = CustomerList.Select(x => x.getCustomerID()).ToList();
                comboBox2.DataSource = AtribList;


            }

        }
        //кнопка для згортання відкритих об'єктів для додавання
        private void InfoButton_Click(object sender, EventArgs e)
        {
            OrderGB.Show(); OrderGB.Height = 33; OrderGB.Location = new Point(10, 105);
            AgronomistGB.Show(); AgronomistGB.Height = 33; AgronomistGB.Location = new Point(10, 70);
            CustomerBox.Show(); CustomerBox.Height = 33; CustomerBox.Location = new Point(10, 36);
        }
        public void PrintWithTypewriterEffect(Label label, string textToPrint, int interval)
        {
            label.Font = new Font("Courier New", 14, FontStyle.Bold);
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = interval;
            int currentIndex = 0;

            timer.Tick += (sender, e) =>
            {
                if (currentIndex < textToPrint.Length)
                {
                    label.Text += textToPrint[currentIndex];
                    currentIndex++;
                }
                else
                {
                    timer.Stop();
                }
            };
            timer.Start();
        }  //метод для ефектного виведення 
        //Мето для фото
        public void MethodForPhotos(string path, PictureBox PB, int positionX)
        {
            Image image = Image.FromFile(path);
            PB.Image = image;
            PB.SizeMode = PictureBoxSizeMode.StretchImage;
            int W = PB.Width, H = PB.Height;

            PB.MouseDown += (sender, e) =>
            {
                panel3.Show();
                PBBig.Image = image;
                PBBig.SizeMode = PictureBoxSizeMode.StretchImage;

            };
            PBBig.MouseUp += (sender, e) =>
            {
                panel3.Hide();
                PBBig.Image = null;

            };

        }

        private void AboutAuthor(object sender, EventArgs e)
        {
            MessageBox.Show("Автором, даного застосунку, є  Перевертень Вадим," +
                "\nСтудент групи АС-225, НУ \"ОП\"\nІнтелектуальна власність!", "Про автора",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button5_Click(object sender, EventArgs e) // про додаток
        {
            MessageBox.Show("SmartAgro - це застосунок для реєстрації " +
                "та обробки інформації про замовлення у агрономічній сфері." +
                "\nДаний застосунок, допоможе легше реєструвати та анілзувати данні про замовлення" +
                "\nУ програмі буде доступно досить таки багато запитів до сисетми, котрі будуть " +
                "генерувати певні потрібні звіти." +
                "\n\n\nНевже майбутнє вже настало? \"для агро бізнесу звісно)\"",
                "Що таке SamrtAgro?", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void button8_Click(object sender, EventArgs e) // зміна шляху запису
        {
            DialogResult dialog = MessageBox.Show("За базовим налаштуванням," +
                "всі файли записуються за шляхом D:\\, якщо ви натисните \"так\", " +
                "то зможете вибрати новий шлях для запису, ТА ВСІ ПОПЕРЕДНІ ЗАПИСИ БУДУТЬ ВИДАЛЕНІ, також, " +
                "програма закриється автоматично, запустіть програму для подальшої роботи.\n Тож Ви впевнені, " +
                "що бажаєте змінити місце для запису програмних файлів?",
                "Зверніть увагу!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dialog == DialogResult.Yes)
            {
                try
                {
                    if (File.Exists(TakePath() + "/AgronomistFile.txt"))
                    { File.Delete(TakePath() + "/AgronomistFile.txt"); }
                    if (File.Exists(TakePath() + "/CustomerFile.txt"))
                    { File.Delete(TakePath() + "/CustomerFile.txt"); }
                    if (File.Exists(TakePath() + "/OrderFile.txt"))
                    { File.Delete(TakePath() + "/OrderFile.txt"); }

                    using (FolderBrowserDialog FBD = new FolderBrowserDialog())
                    {
                        DialogResult res = FBD.ShowDialog();
                        if (res == DialogResult.OK && !string.IsNullOrWhiteSpace(FBD.SelectedPath))
                        {
                            string path = FBD.SelectedPath.ToString();
                            Registry.CurrentUser.CreateSubKey(@"Software\КурсоваРобота").SetValue("path", path);
                        }
                    }
                }
                catch (Exception) { MessageBox.Show("ErroR"); }
                this.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e) // як використовувати
        {
            string Text = "Автор даного застосунку, намагався зробити інтелектуально простим інтерфейс, " +
                "та це вікно допоможе вам зорієнтувтуватися у застосунку. " +
                "Зараз ви знаходитесь у головному меню, зверху ви можете спостерігати певні " +
                "вкладки із різними назвами, дані вкладки мають весь функціонал програми" +
                "досить просто натиснути на дану вкладку і ви матимите доступ до іншої вкладки(нічого складного)" +
                "далі інші вкладки матимуть свої правила використання, варто просто натиснути кнопку \"Справка\"" +
                "\nНа даній вкладці маємо прості кнопки та фотогалерею" +
                "Галерея має можливість прокручування та збільшення-зменшення фото по одному натисканню ПКМ." +
                "На сьогодні це весь доступний функціонал" +
                "(ну і ще можна натиснути на кнопку автора внизу😬)";
            MessageBox.Show(Text, "Як користуватися?", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button3_Click(object sender, EventArgs e) // справка для довання
        {
            string Text = "Даний розділ програми відповідає за додавання нових замовлень, клієнтів, агрономів" +
                "Є одним із важливих розділів, так як саме тут заноситься інформація котра буде опрацьовуватись" +
                "Коректне введення є дуже важливим, тому що саме від коректно введеної інформації залежить " +
                "відповідь на запити до системи\n" +
                "Правила: Спершу варто обрати кого ви хочете додати, потім у коректному форматі заповнити дані" +
                "у порядку розміщення. Після заповнення ВСІХ полів варто натиснути кнопку \"Додати...\"" +
                "У разі певного некоректного введення або непотрібного, можна натиснути кнопку \"Очистити поля\"" +
                "Коли ви будете некоректно вводити значення або данні вас програма повідомить про це, " +
                "та все ж варто дотримуватись правил!" +
                "Доьримуйтесь правил, та слухайте маму)" +
                "ДЯКУЮ ЗА УВАГУ!";
            MessageBox.Show(Text, "Які такі запити?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button13_Click(object sender, EventArgs e) // про запити до системи
        {
            string Text = "1. Список рослин до видалення\n(список рослин які видаляються із кожного замовлення)" +
                "\n2. Список рослин до посадження(схоже до попереднього)" +
                "\n3. Список клієнтів які обрали певний тип послуги(спок клієнтів де вказаний кожен клієнт" +
                "та його обраний тип послуги)" +
                "\n4. Середня вартість замовлення (обрахована середня вартість на замовлення та " +
                "виведене надорожче та найдешевше замовлення)" +
                "\n5. Середній час на виконання замовлення, середня значення часу, для виконання замовлення" +
                "\n6. Замовлення із найбільшою ціною (Виведення повної інформації про замолення)";
            MessageBox.Show(Text, "Які такі запити?", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void AddButton_Click(object sender, EventArgs e) // додати замовника
        {
            string SecName = "", FirstName = "", FathersName = "", adress = "", phoneNumber = "", ID = "",
                AgronomistFN = "";
            Random random = new Random();
            try
            {
                if (!string.IsNullOrEmpty(SurnameTB.Text) && !string.IsNullOrEmpty(NameTB.Text)
                    && !string.IsNullOrEmpty(FatherNameTB.Text) && !string.IsNullOrEmpty(AdressTB.Text)
                    && !string.IsNullOrEmpty(PHTB.Text))
                {
                    SecName = SurnameTB.Text + " ";
                    FirstName = NameTB.Text + " ";
                    FathersName = FatherNameTB.Text;
                    adress = AdressTB.Text;
                    phoneNumber = PHTB.Text;
                    ID = random.Next(100000, 999999).ToString();
                    GeneretedID.Text = ID;
                    int randIndex = random.Next(0, AgronomistList.Count);
                    AgronomistFN = AgronomistList[randIndex].getPeopleFullName();
                    label30.Text = AgronomistFN;
                    methodForAddingCustomer(SecName + FirstName + FathersName, phoneNumber, adress, ID,
                        AgronomistFN);
                    string info = SecName + FirstName + FathersName + "/" + adress + "/" + phoneNumber + "/"
                        + ID + "/" + AgronomistFN;
                    WriteToFile WTF = new WriteToFile(info, 3);
                }
                else
                {
                    MessageBox.Show("Виникла помилка при додаванні, впевніться, будь ласка, " +
                    "чи всі поля заповнені", "Помилка 😬",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (System.IndexOutOfRangeException)
            {
                MessageBox.Show("Виникла помилка при додаванні, ви внесли забагто символів, будь ласка, " +
                    "зменшіть кількість симвоів та спробуйте ще раз", "Помилка 😬",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void button2_Click(object sender, EventArgs e)// очищення полів для замовника
        {
            SurnameTB.Clear();
            NameTB.Clear();
            FatherNameTB.Clear();
            AdressTB.Clear();
            PHTB.Clear();
            GeneretedID.Text = "";
            label30.Text = "";
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                richTextBox2.Text = "";
                foreach (Customer CL in CustomerList)
                {
                    richTextBox2.Text += CL.GiveInfo() + "\n-----------------------------------\n";
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                richTextBox2.Text = "";
                foreach (Agronomist CL in AgronomistList)
                {
                    richTextBox2.Text += CL.GiveInfo() + "\n-----------------------------------\n";
                }
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                richTextBox2.Text = "";
                foreach (Order CL in OrderList)
                {
                    richTextBox2.Text += CL.GiveInfo() + "\n-----------------------------------\n";
                }
            }
        } // звіт список

        private void AgronomistAddB_Click(object sender, EventArgs e) // додати агронома
        {
            string AFN = "", PN = "";
            try
            {
                if (!string.IsNullOrEmpty(AgronomistSurname.Text) && !string.IsNullOrEmpty(AgronomistName.Text)
                    && !string.IsNullOrEmpty(FatersNameAgronomist.Text) && !string.IsNullOrEmpty(AgronomistPN.Text))
                {
                    AFN = AgronomistSurname.Text.ToString() + " " + AgronomistName.Text.ToString() + " " + FatersNameAgronomist.Text.ToString();
                    PN = AgronomistPN.Text;
                    FilialNameAgronomist.Text = Agronomist.getBrunchName();

                    methodForAddingAgronomist(AFN, PN);
                    string info = AFN + "/" + PN + "/" +
                        Agronomist.getBrunchName();
                    WriteToFile WTF = new WriteToFile(info, 2);
                }
                else
                {
                    MessageBox.Show("Виникла помилка при додаванні, впевніться, будь ласка, " +
                    "чи всі поля заповнені", "Помилка 😬",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Виникла невідома помилка, " +
                    "агронома не було додано, спробуйте ще раз пізніше", "Помилка 😬",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button4_Click(object sender, EventArgs e) // очистити поля для агронома
        {
            AgronomistSurname.Clear();
            AgronomistName.Clear();
            FatersNameAgronomist.Clear();
            AgronomistPN.Clear();
            FilialNameAgronomist.Text = "";
        }

        private void MakeOrderButton_Click(object sender, EventArgs e)// додати замовлення
        {
            string Adres = "", TypeOfPlant = "", DateOfStart = "", DateOfEnd = "", IDOfOrder = "",
                IDOfCustomer = "";
            bool TypeOfService = false, AllIsGood1 = false, AllIsGood2 = false, AllIsGood3 = false,
                AllIsGood4 = false;
            double Size = 0, price = 0;
            Random rand = new Random();
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text)
                    && !string.IsNullOrEmpty(textBox3.Text))
                {
                    Adres = textBox1.Text;
                    Size = double.Parse(textBox2.Text);
                    price = double.Parse(textBox3.Text);
                    AllIsGood1 = true;
                }
                else
                {
                    MessageBox.Show("Виникла помилка при додаванні, впевніться, будь ласка, " +
                    "чи всі поля заповнені", "Помилка 😬",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AllIsGood1 = false;

                }
                if (DeletedRBOrder.Checked) { TypeOfService = false; AllIsGood2 = true; }
                else if (PlantRBOrder.Checked) { TypeOfService = true; AllIsGood2 = true; }
                else
                {
                    MessageBox.Show("Виникла помилка при додаванні, ви не обрали тип роботи, будь ласка, " +
                    "всі поля заповніть", "Помилка 😬",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AllIsGood2 = false;
                }
                if (CombBoxOrder.SelectedIndex == 0) { TypeOfPlant = "Дерева"; AllIsGood3 = true; }
                else if (CombBoxOrder.SelectedIndex == 1) { TypeOfPlant = "Кущі"; AllIsGood3 = true; }
                else if (CombBoxOrder.SelectedIndex == 2) { TypeOfPlant = "Однорічні рослини"; AllIsGood3 = true; }
                else if (CombBoxOrder.SelectedIndex == 3) { TypeOfPlant = "Багаторічні рослини"; AllIsGood3 = true; }
                else if (CombBoxOrder.SelectedIndex == 4) { TypeOfPlant = "Злакові"; AllIsGood3 = true; }
                else if (CombBoxOrder.SelectedIndex == -1)
                {
                    MessageBox.Show("Виникла помилка при додаванні, ви не обрали тип рослиин, будь ласка, " +
                    "заповніть всі поля", "Помилка 😬",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AllIsGood3 = false;
                }

                DateOfStart = StartDatePicker.Text.ToString();
                DateOfEnd = EndDatePicker.Text.ToString();
                DateTime d1 = DateTime.ParseExact(DateOfStart, "dd.MM.yyyy", null);
                DateTime d2 = DateTime.ParseExact(DateOfEnd, "dd.MM.yyyy", null);
                if (d1 < d2) { AllIsGood4 = true; }
                else { MessageBox.Show("Різниця між датою початку та кінця має бути мінімум один день!", "Проблема із часом", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                IDOfCustomer = CustomerList[comboBox2.SelectedIndex].getCustomerID();

            }
            catch (System.FormatException)
            {

                MessageBox.Show("Виникла помилка, у поях де повинні були бути числа був текст " +
                "замовлення не було додано, спробуйте ще раз", "Помилка 😬",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            catch (Exception)
            {
                MessageBox.Show("Виникла невідома помилка, " +
                                "замовлення не було додано, спробуйте ще раз", "Помилка 😬",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            if (AllIsGood1 && AllIsGood2 && AllIsGood3 && AllIsGood4)
            {
                IDOfOrder = rand.Next(100000, 999999).ToString();
                GeneretedOrderID.Text = IDOfOrder;
                string info = TypeOfPlant + "/" + DateOfStart + "/" + DateOfEnd + "/"
                    + TypeOfService.ToString() + "/" + price.ToString() + "/" + Size.ToString()
                    + "/" + Adres + "/" + IDOfCustomer + "/" + IDOfOrder;
                methodForAddingOrder(Adres, TypeOfPlant, DateOfStart, DateOfEnd, TypeOfService,
                    Size, price, IDOfOrder, IDOfCustomer);
                WriteToFile WTF = new WriteToFile(info, 1);

            }

        }

        private void button6_Click(object sender, EventArgs e) // очистити поля для замовлення
        {
            GeneretedOrderID.Text = "";
            textBox3.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
            PlantRBOrder.Checked = false;
            DeletedRBOrder.Checked = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
        } // очистити список об'єктів

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) // обираємо замовлення для замовника
        {

            if (comboBox2.SelectedIndex <= CustomerList.Count)
            {
                PickedCustomerIDInOrder.Text = CustomerList[comboBox2.SelectedIndex].getPeopleFullName();
            }

        }

        private void button10_Click(object sender, EventArgs e) // довідка по  перегляду
        {
            MessageBox.Show("У даному розділі можна переглянути всіх раніше доданих," +
                "осіб або замовлень\nЛише обреіть осписку кого хочете побачити," +
                "та далі вся інформація виведеться", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("У даному розділі можна переглянути запити до системи(певна згенерована статистика)" +
                "Про запити можна прочитати  у головному меню, натиснувши відповідну кнопку \"Запити до  системи\"" +
                "Щоб отримати інформацію просто відкрийте пронумерований список та оберіть запит за бажанням.",
                "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }// довідка по запитах
        //Запити до системи
        private void RTScb(object sender, EventArgs e)
        {
            if (RequestComboBox.SelectedIndex == 0)
            {
                string Result = "";
                foreach (Order o in OrderList)
                {
                    if (o.TypeOfService == false)
                    {
                        Result += "Рослина: " + o.typeOfPlant + "; ID замовлення: " + o.IDOfOrder + "\n-----------------\n";
                    }
                }
                richTextBox1.Text = Result;
            } //перший запит
            else if (RequestComboBox.SelectedIndex == 1)
            {
                string Result = "";
                foreach (Order o in OrderList)
                {
                    if (o.TypeOfService)
                    {
                        Result += "Рослина: " + o.typeOfPlant + "; ID замовлення: " + o.IDOfOrder + "\n-----------------\n";
                    }
                }
                richTextBox1.Text = Result;

            } // другий запит
            else if (RequestComboBox.SelectedIndex == 2)
            {
                string Result = "";
                foreach (Order o in OrderList)
                {
                    string TOS = o.TypeOfService ? "Посадження" : "Видалення";
                    Result += "ID замовника: " + o.getCustomerID() +
                        "\nID замовлення: " + o.IDOfOrder + "\nТип послуги: " + TOS;
                    Result += "\n------------------\n";
                }


                richTextBox1.Text = Result;
            } // третій запит
            else if (RequestComboBox.SelectedIndex == 3)
            {
                string Result = "Середня ціна замовлення: ";
                Result += Math.Round(OrderList.Average(pr => pr.price), 2).ToString() + " грн";
                double maxPrice = Math.Round(OrderList.Max(pr => pr.price), 2);
                double minPrice = Math.Round(OrderList.Min(pr => pr.price), 2);
                foreach (Order o in OrderList)
                {
                    if (o.price == maxPrice)
                    {
                        Result += "\nМаксимальна ціна: " + maxPrice.ToString() + "; ID цього замволення: "
                            + o.IDofCustomerThatMadeThisOrder;
                    }
                    if (o.price == minPrice)
                    {
                        Result += "\nМінімальна ціна: " + minPrice.ToString() + "; ID цього замволення: "
                                + o.IDOfOrder;

                    }
                }
                richTextBox1.Text = Result;
            } // четвертий запит
            else if (RequestComboBox.SelectedIndex == 4)
            {
                if (OrderList.Any())
                {
                    double result = 0;
                    for (int i = 0; i < OrderList.Count; i++)
                    {
                        result += Double.Parse(((DateTime.Parse(OrderList[i].DateOfEnd)
                        - (DateTime.Parse(OrderList[i].DateOfStart))).TotalHours).ToString());
                    }
                    result = result / OrderList.Count;
                    if (result >= 24)
                    {
                        result = Math.Round(result / 24, 0);
                        richTextBox1.Text = "Середній час на виконання одного замовлення у днях: "
                            + result.ToString();
                    }
                    else
                    {
                        richTextBox1.Text = "Середній час на виконання одного замовлення у годинах: "
                            + result.ToString();

                    }
                }
            } // п'ятий запит
            else if (RequestComboBox.SelectedIndex == 5)
            {
                richTextBox1.Text = "Замовлення із найбільшою ціною\n\n";
                double maxPrice = Math.Round(OrderList.Max(pr => pr.price), 2);
                foreach (Order o in OrderList)
                {
                    if (o.price == maxPrice) { richTextBox1.Text += o.GiveInfo(); }
                }
            } // шостий запит
        }

        private void button11_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        } // кнопка для очищення
    }
}
