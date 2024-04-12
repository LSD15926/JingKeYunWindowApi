namespace APIOffice.Controllers.pddApi
{
    public class BackMsg
    {
        public int Code { get; set; }
        public string Mess { get; set; } = "";
    }

    public class BackData
    {
        public int Code { get; set; }
        public string Mess { get; set; } = "";
        public List<object> Data { get; set; }

    }

}
