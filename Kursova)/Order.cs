public class Order : ISphereOfOrder
{
    private string adress;
    public string typeOfPlant, DateOfStart, DateOfEnd, IDOfOrder, IDofCustomerThatMadeThisOrder;
    public bool TypeOfService;
    public double Size, price;
    public Order(string adress, string typeOfPlant , string DateOfStart, string DateOfEnd, 
        double Size, double price, string IDOfOrder, string IDofCustomerThatMadeThisOrder, 
        bool TypeOfService)
    {
        this.adress = adress;
        this.typeOfPlant = typeOfPlant;
        this.DateOfStart = DateOfStart;
        this.DateOfEnd = DateOfEnd;
        this.TypeOfService = TypeOfService;
        this.Size = Size;
        this.IDOfOrder = IDOfOrder;
        this.IDofCustomerThatMadeThisOrder = IDofCustomerThatMadeThisOrder;
        this.price = price;

    }
     


    public string getAdress() { return adress; }
    public string getCustomerID() { return IDofCustomerThatMadeThisOrder; }

    public void setAdress(string adress) { this.adress = adress; }
    public void setCustomerID (string CustID) { this.IDofCustomerThatMadeThisOrder = CustID; }

    public string GiveInfo()
    {
        string TOS = TypeOfService ? "посадження" : "видалення";

        string FullInfoAbout =  "Тип рослини: " + typeOfPlant +
            "\nДата початку: " + DateOfStart +
            "\nДата кінця: " + DateOfEnd +
            "\nТип послуги: " + TOS +
            "\nРозмір роботи: " + Size + " м^2" +
            "\nЦіна за виконану роботу: " + price +
            "\nАдерс за яким викнувалась робота: " + getAdress() +
            "\nID замовника, котрий зробив це замовлення: " + getCustomerID() +
            "\nID замовлення: " + IDOfOrder;
        return FullInfoAbout;
    }

}

