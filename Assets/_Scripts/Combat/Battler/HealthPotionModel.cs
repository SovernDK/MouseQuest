public class HealthPotionModel
{
    private int _current;
    private int _max;
    private int _healValue;
    private string _name;

    public int Drink()
    {
        if(_current > 0)
            return _healValue;
        
        return -1;
    }

    public void Refill()
    {
        _current = _max;
    }

    public void Set(string name, int value)
    {
        _name = name;
        _healValue = value;
    }
}