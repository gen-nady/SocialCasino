
//Post на отправку баланса. Ответ не нужен.

using System;
using System.Collections.Generic;

public class BalanceVm
{
    public string UserId { get; set; }
    public decimal Balance { get; set; }
}


//post на полчение лидеров
public class LidersVm
{
    public string UserId { get; set; }
    public LastTime LastTime { get; set; }
}

public enum LastTime
{
    None,
    Last30Days,
    Last7Days,
    Last1Days,
}

//ответ на полчение лидеров
public class LidersQuery
{
   public List<Lider> Liders { get; set; }
}

public class Lider
{
    public string UserName { get; set; }
    public decimal Balance { get; set; }
}


