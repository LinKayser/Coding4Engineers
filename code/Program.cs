try
{
    PicoGK.Library.Go(  .5f, 
                        Coding4Engineers.Chapter11.FixtureMakerApp.Run);
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}