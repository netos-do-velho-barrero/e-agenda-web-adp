using System.Data;
using Dapper;

namespace eAgenda.WebApp.Compartilhado.Infra.Sql;

public static class DapperTypeHandlers
{
    public static void Registrar()
    {
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new NullableDateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new NullableTimeOnlyTypeHandler());
    }
}

public sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        return value switch
        {
            DateTime data => DateOnly.FromDateTime(data),
            DateOnly data => data,
            _ => DateOnly.Parse(value.ToString()!)
        };
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }
}

public sealed class NullableDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly?>
{
    public override DateOnly? Parse(object value)
    {
        if (value is null || value is DBNull)
            return null;

        return value switch
        {
            DateTime data => DateOnly.FromDateTime(data),
            DateOnly data => data,
            _ => DateOnly.Parse(value.ToString()!)
        };
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly? value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.HasValue
            ? value.Value.ToDateTime(TimeOnly.MinValue)
            : DBNull.Value;
    }
}

public sealed class TimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
{
    public override TimeOnly Parse(object value)
    {
        return value switch
        {
            TimeSpan horario => TimeOnly.FromTimeSpan(horario),
            TimeOnly horario => horario,
            DateTime horario => TimeOnly.FromDateTime(horario),
            _ => TimeOnly.Parse(value.ToString()!)
        };
    }

    public override void SetValue(IDbDataParameter parameter, TimeOnly value)
    {
        parameter.DbType = DbType.Time;
        parameter.Value = value.ToTimeSpan();
    }
}

public sealed class NullableTimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly?>
{
    public override TimeOnly? Parse(object value)
    {
        if (value is null || value is DBNull)
            return null;

        return value switch
        {
            TimeSpan horario => TimeOnly.FromTimeSpan(horario),
            TimeOnly horario => horario,
            DateTime horario => TimeOnly.FromDateTime(horario),
            _ => TimeOnly.Parse(value.ToString()!)
        };
    }

    public override void SetValue(IDbDataParameter parameter, TimeOnly? value)
    {
        parameter.DbType = DbType.Time;
        parameter.Value = value.HasValue
            ? value.Value.ToTimeSpan()
            : DBNull.Value;
    }
}
