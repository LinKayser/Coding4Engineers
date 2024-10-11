try
{
    PicoGK.Library.Go(  .5f, 
                        Coding4Engineers.Chapter12.FixtureMakerApp.Run);
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}