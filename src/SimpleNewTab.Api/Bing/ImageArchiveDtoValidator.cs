namespace SimpleNewTab.Api.Bing
{
    public sealed class ImageArchiveDtoValidator : AbstractValidator<ImageArchiveDto>
    {
        public ImageArchiveDtoValidator()
        {
            RuleFor(response => response.Images)
                .Must(x => x != null && x.Count() == 1)
                .WithMessage("'Images' must contain exactly one element.");
            RuleForEach(response => response.Images)
                .ChildRules(image =>
                {
                    image.RuleFor(x => x.Url)
                        .NotEmpty()
                        .Must(x => x.StartsWith('/'))
                        .WithMessage("'Url' must be a relative route.");
                    image.RuleFor(x => x.Title)
                        .NotEmpty();
                    image.RuleFor(x => x.Quiz)
                        .NotEmpty()
                        .Must(x => x.StartsWith('/'))
                        .WithMessage("'Quiz' must be a relative route.");
                    image.RuleFor(x => x.Copyright)
                        .NotEmpty();
                    image.RuleFor(x => x.CopyrightLink)
                        .NotEmpty()
                        .Must(x => x.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                        .WithMessage("'Copyright Link' must be a valid url.");
                });
        }
    }
}
