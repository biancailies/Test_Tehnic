using System.Net.Http.Headers;

class Products
{
    public enum Product { TV, PC, mouse, keyboard, laptop, display};
}

class OrderItem
{
    Products.Product product;
    int quantity;
    decimal price;

    public OrderItem(Products.Product product, int quantity, decimal price)
    {
        this.product = product;
        this.quantity = quantity;
        this.price = price;
    }

    public Products.Product Product
    {
        get { return product; }
    }

    public decimal Price
    {
        get { return price; }
    }

    public int Quantity
    {
        get { return quantity; }
    }
}

class Order
{
    List<OrderItem> productsList = new List<OrderItem>();
    bool discount = false;

    public Order(List<OrderItem> productsList)
    {
        this.productsList = productsList;
    }

    public decimal PriceCalc()
    {
        int length = productsList.Count;
        decimal finalCost = 0.0m;

        for (int i = 0; i < length; i++)
        {
            finalCost += productsList[i].Price * productsList[i].Quantity;
        }

        if(finalCost > 500)
        {
            finalCost = finalCost - (finalCost * 0.1m);
            discount = true;
        }

        return finalCost;
    }

    public List<OrderItem> ProductsList
    {
        get { return productsList; }
    }

    public bool Discount    
    {
        get { return discount; }
    }
}

class Customer
{
    String name;
    List<Order> orders = new List<Order>();

    public Customer(string name, List<Order> orders)
    {
        this.name = name;
        this.orders = orders;
    }

    public void AddOrder(Order o)
    {
        orders.Add(o);
    }

    public List<Order> Orders
    {
        get { return orders; }
    }

    public string Name
    {
        get { return name; }
    }

}

class Store
{
    List<Customer> customersList= new List<Customer>();

    public Store(List<Customer> customersList)
    {
        this.customersList = customersList;
    }

    public string MostSpent()
    {
        int numberOfCustomers = customersList.Count;
        decimal totalPrice = 0.0m;
        int customerIndex = 0;
        decimal most = 0.0m;

        for (int customer = 0; customer < numberOfCustomers; customer++)
        {
            totalPrice = 0.0m;
            List<Order> orders = customersList[customer].Orders;
            int numberOfOrders = orders.Count;

            for (int order = 0; order < numberOfOrders; order++)
            {
                decimal totalPricePerOrder = 0.0m;

                totalPricePerOrder += orders[order].PriceCalc();
                totalPrice += totalPricePerOrder;
            }

            if(totalPrice > most)
            {
                most = totalPrice;
                customerIndex = customer;
            }
        }

        return customersList[customerIndex].Name;
    }

    public List<KeyValuePair<Products.Product, int>> MostPopular()
    {
        Dictionary<Products.Product, int> dict = new Dictionary<Products.Product, int>();

        List<KeyValuePair<Products.Product, int>> mostPopular = new List<KeyValuePair<Products.Product, int>>();
        int numberOfCustomers = customersList.Count;

        for (int customer = 0; customer < numberOfCustomers; customer++)
        {
            List<Order> orders = customersList[customer].Orders;
            int numberOfOrders = orders.Count;

            for (int order = 0; order < numberOfOrders; order++)
            {
                int numberOfProducts = orders[order].ProductsList.Count;

                for(int prod = 0; prod < numberOfProducts; prod++)
                {
                    if (dict.ContainsKey(orders[order].ProductsList[prod].Product))
                    {
                        dict[orders[order].ProductsList[prod].Product] += orders[order].ProductsList[prod].Quantity;
                    }
                    else
                    {
                        dict[orders[order].ProductsList[prod].Product] = orders[order].ProductsList[prod].Quantity;
                    }
                }

            }

        }

        mostPopular = dict
                         .OrderByDescending(x => x.Value)
                         .ToList();

        return mostPopular;
    }

}

class Program
{
    static void Main(string[] args)
    {
        //order1
        OrderItem item1 = new OrderItem(Products.Product.TV, 2, 150);
        OrderItem item2 = new OrderItem(Products.Product.laptop, 1, 200);

        List<OrderItem> items = new List<OrderItem>();
        items.Add(item1);
        items.Add(item2);

        Order o1 = new Order(items);
        List<Order> order1 = new List<Order>();
        order1.Add(o1);

        //order2
        OrderItem item3 = new OrderItem(Products.Product.laptop, 1, 200);
        OrderItem item4 = new OrderItem(Products.Product.mouse, 1, 50);
        OrderItem item5 = new OrderItem(Products.Product.keyboard, 1, 135);

        List<OrderItem> items2 = new List<OrderItem>();
        items2.Add(item3);
        items2.Add(item4);
        items2.Add(item5);

        Order o2 = new Order(items2);
        List<Order> order2 = new List<Order>();
        order2.Add(o2);

        //order3
        OrderItem item6 = new OrderItem(Products.Product.PC, 1, 1000);

        List<OrderItem> items3 = new List<OrderItem>();
        items3.Add(item6);

        Order o3 = new Order(items3);
        List<Order> order3 = new List<Order>();
        order3.Add(o3);

        //order4
        OrderItem item7 = new OrderItem(Products.Product.display, 1, 550);

        List<OrderItem> items4 = new List<OrderItem>();
        items4.Add(item7);

        Order o4 = new Order(items4);
        order1.Add(o4);

        Customer John = new Customer("John", order1);
        Customer Bob = new Customer("Bob", order2);
        Customer Anna = new Customer("Anna", order3);

        List<Customer> customersList = new List<Customer>();

        customersList.Add(John);
        customersList.Add(Anna);
        customersList.Add(Bob);

        Store store = new Store(customersList);

        int numberOfCustomers = customersList.Count;

        for(int customer = 0; customer < numberOfCustomers; customer++)
        {
            List<Order> orders = customersList[customer].Orders;
            int numberOfOrders = orders.Count;

            for (int order = 0; order < numberOfOrders; order++)
            {
                decimal totalPricePerOrder = 0.0m;

                totalPricePerOrder += orders[order].PriceCalc();

                if (orders[order].Discount)
                {
                    Console.WriteLine("For customer: " + customersList[customer].Name +
                        " the total for order number: " + order + " is:" + totalPricePerOrder + " after discount\n");
                }
                else
                {
                    Console.WriteLine("For customer: " + customersList[customer].Name +
                        " the total for order number: " + order + " is:" + totalPricePerOrder + ", discount was not possible\n");
                }
                    
            }

        }

        Console.WriteLine("The customer who has spent the most money on all their orders is: " 
            + store.MostSpent());

        List<KeyValuePair<Products.Product, int>> mostPopular = store.MostPopular();

        Console.WriteLine("\nThe most popular products:");

        int length = mostPopular.Count;

        for (int i = 0; i < length; i++)
        {
            Console.WriteLine("Product: " + mostPopular[i].Key + " quantity: " + mostPopular[i].Value);
        }
    }
}