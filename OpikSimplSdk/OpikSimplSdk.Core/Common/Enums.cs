namespace OpikSimplSdk.Core.Common;

public enum FeedbackScoreSource
{
    Ui,
    Sdk,
    OnlineScoring
}

public enum SpanType
{
    General,
    Tool,
    Llm,
    Guardrail
}

public enum FeedbackDefinitionType
{
    Numerical,
    Categorical
}

public enum Visibility
{
    Private,
    Public
}

public enum ExperimentStatus
{
    Running,
    Completed,
    Cancelled
}

public enum ExperimentType
{
    Regular,
    Trial,
    MiniBatch
}

public enum PromptType
{
    Mustache,
    Jinja2
}

public enum EntityType
{
    Trace,
    Span
}
