namespace EX.Common.Dtos
{
    public class SelectItemDto
    {

        public string Text { get; set; }

        public string Value { get; set; }

        public SelectItemDto()
        {

        }

        public SelectItemDto(string text, string value)
        {
            Text = text;
            Value = value;
        }


        public override string ToString()
        {
            return $"{Text} ({Value})";
        }
    }
}
