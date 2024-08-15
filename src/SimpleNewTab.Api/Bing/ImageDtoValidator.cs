namespace SimpleNewTab.Api.Bing
{
    public sealed class ImageDtoValidator : AbstractValidator<ImageDto>
    {
        public ImageDtoValidator()
        {
            RuleFor(x => x.Url)
                .Matches("^/");
            RuleFor(x => x.Title)
                .NotEmpty();
            RuleFor(x => x.Quiz)
                .Matches("^/");
            RuleFor(x => x.Copyright)
                .NotEmpty();
            RuleFor(x => x.CopyrightLink)
                .Matches("^http");
        }
    }
}
