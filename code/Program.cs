try
{
    PicoGK.Library.Go(  .1f, 
                        Coding4Engineers.Chapter16.QuadSubdivision.RunGauss);
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}