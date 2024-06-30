try
{
    PicoGK.Library.Go(  0.5f, 
                        Coding4Engineers.Chapter10.Fixtures.App.Run);
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}