try
{
    PicoGK.Library.Go(  .1f, 
                        Coding4Engineers.Chapter14.LatticeMap.Run);
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}