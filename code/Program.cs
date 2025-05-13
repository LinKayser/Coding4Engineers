try
{
    PicoGK.Library.Go(  .1f, 
                        Coding4Engineers.AnimatedObject.App.Run);
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}