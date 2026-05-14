FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Central build props (must be present before dotnet restore)
COPY Directory.Build.props .
COPY Directory.Packages.props .

# Core projects
COPY ["src/Kamirion.RepairShop.Shared/Kamirion.RepairShop.Shared.csproj", "src/Kamirion.RepairShop.Shared/"]
COPY ["src/Kamirion.RepairShop.Infrastructure/Kamirion.RepairShop.Infrastructure.csproj", "src/Kamirion.RepairShop.Infrastructure/"]
COPY ["src/Kamirion.RepairShop.Web/Kamirion.RepairShop.Web.csproj", "src/Kamirion.RepairShop.Web/"]

# Analytics
COPY ["src/Modules/Kamirion.RepairShop.Analytics/Kamirion.RepairShop.Analytics.Contracts/Kamirion.RepairShop.Analytics.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Analytics/Kamirion.RepairShop.Analytics.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Analytics/Kamirion.RepairShop.Analytics.Domain/Kamirion.RepairShop.Analytics.Domain.csproj", "src/Modules/Kamirion.RepairShop.Analytics/Kamirion.RepairShop.Analytics.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Analytics/Kamirion.RepairShop.Analytics.Application/Kamirion.RepairShop.Analytics.Application.csproj", "src/Modules/Kamirion.RepairShop.Analytics/Kamirion.RepairShop.Analytics.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Analytics/Kamirion.RepairShop.Analytics.Infrastructure/Kamirion.RepairShop.Analytics.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Analytics/Kamirion.RepairShop.Analytics.Infrastructure/"]

# Audit
COPY ["src/Modules/Kamirion.RepairShop.Audit/Kamirion.RepairShop.Audit.Contracts/Kamirion.RepairShop.Audit.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Audit/Kamirion.RepairShop.Audit.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Audit/Kamirion.RepairShop.Audit.Domain/Kamirion.RepairShop.Audit.Domain.csproj", "src/Modules/Kamirion.RepairShop.Audit/Kamirion.RepairShop.Audit.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Audit/Kamirion.RepairShop.Audit.Application/Kamirion.RepairShop.Audit.Application.csproj", "src/Modules/Kamirion.RepairShop.Audit/Kamirion.RepairShop.Audit.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Audit/Kamirion.RepairShop.Audit.Infrastructure/Kamirion.RepairShop.Audit.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Audit/Kamirion.RepairShop.Audit.Infrastructure/"]

# Automation
COPY ["src/Modules/Kamirion.RepairShop.Automation/Kamirion.RepairShop.Automation.Contracts/Kamirion.RepairShop.Automation.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Automation/Kamirion.RepairShop.Automation.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Automation/Kamirion.RepairShop.Automation.Domain/Kamirion.RepairShop.Automation.Domain.csproj", "src/Modules/Kamirion.RepairShop.Automation/Kamirion.RepairShop.Automation.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Automation/Kamirion.RepairShop.Automation.Application/Kamirion.RepairShop.Automation.Application.csproj", "src/Modules/Kamirion.RepairShop.Automation/Kamirion.RepairShop.Automation.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Automation/Kamirion.RepairShop.Automation.Infrastructure/Kamirion.RepairShop.Automation.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Automation/Kamirion.RepairShop.Automation.Infrastructure/"]

# Communication
COPY ["src/Modules/Kamirion.RepairShop.Communication/Kamirion.RepairShop.Communication.Contracts/Kamirion.RepairShop.Communication.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Communication/Kamirion.RepairShop.Communication.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Communication/Kamirion.RepairShop.Communication.Domain/Kamirion.RepairShop.Communication.Domain.csproj", "src/Modules/Kamirion.RepairShop.Communication/Kamirion.RepairShop.Communication.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Communication/Kamirion.RepairShop.Communication.Application/Kamirion.RepairShop.Communication.Application.csproj", "src/Modules/Kamirion.RepairShop.Communication/Kamirion.RepairShop.Communication.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Communication/Kamirion.RepairShop.Communication.Infrastructure/Kamirion.RepairShop.Communication.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Communication/Kamirion.RepairShop.Communication.Infrastructure/"]

# Configuration
COPY ["src/Modules/Kamirion.RepairShop.Configuration/Kamirion.RepairShop.Configuration.Contracts/Kamirion.RepairShop.Configuration.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Configuration/Kamirion.RepairShop.Configuration.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Configuration/Kamirion.RepairShop.Configuration.Domain/Kamirion.RepairShop.Configuration.Domain.csproj", "src/Modules/Kamirion.RepairShop.Configuration/Kamirion.RepairShop.Configuration.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Configuration/Kamirion.RepairShop.Configuration.Application/Kamirion.RepairShop.Configuration.Application.csproj", "src/Modules/Kamirion.RepairShop.Configuration/Kamirion.RepairShop.Configuration.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Configuration/Kamirion.RepairShop.Configuration.Infrastructure/Kamirion.RepairShop.Configuration.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Configuration/Kamirion.RepairShop.Configuration.Infrastructure/"]

# Customers
COPY ["src/Modules/Kamirion.RepairShop.Customers/Kamirion.RepairShop.Customers.Contracts/Kamirion.RepairShop.Customers.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Customers/Kamirion.RepairShop.Customers.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Customers/Kamirion.RepairShop.Customers.Domain/Kamirion.RepairShop.Customers.Domain.csproj", "src/Modules/Kamirion.RepairShop.Customers/Kamirion.RepairShop.Customers.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Customers/Kamirion.RepairShop.Customers.Application/Kamirion.RepairShop.Customers.Application.csproj", "src/Modules/Kamirion.RepairShop.Customers/Kamirion.RepairShop.Customers.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Customers/Kamirion.RepairShop.Customers.Infrastructure/Kamirion.RepairShop.Customers.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Customers/Kamirion.RepairShop.Customers.Infrastructure/"]

# Devices
COPY ["src/Modules/Kamirion.RepairShop.Devices/Kamirion.RepairShop.Devices.Contracts/Kamirion.RepairShop.Devices.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Devices/Kamirion.RepairShop.Devices.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Devices/Kamirion.RepairShop.Devices.Domain/Kamirion.RepairShop.Devices.Domain.csproj", "src/Modules/Kamirion.RepairShop.Devices/Kamirion.RepairShop.Devices.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Devices/Kamirion.RepairShop.Devices.Application/Kamirion.RepairShop.Devices.Application.csproj", "src/Modules/Kamirion.RepairShop.Devices/Kamirion.RepairShop.Devices.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Devices/Kamirion.RepairShop.Devices.Infrastructure/Kamirion.RepairShop.Devices.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Devices/Kamirion.RepairShop.Devices.Infrastructure/"]

# Identity
COPY ["src/Modules/Kamirion.RepairShop.Identity/Kamirion.RepairShop.Identity.Contracts/Kamirion.RepairShop.Identity.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Identity/Kamirion.RepairShop.Identity.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Identity/Kamirion.RepairShop.Identity.Domain/Kamirion.RepairShop.Identity.Domain.csproj", "src/Modules/Kamirion.RepairShop.Identity/Kamirion.RepairShop.Identity.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Identity/Kamirion.RepairShop.Identity.Application/Kamirion.RepairShop.Identity.Application.csproj", "src/Modules/Kamirion.RepairShop.Identity/Kamirion.RepairShop.Identity.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Identity/Kamirion.RepairShop.Identity.Infrastructure/Kamirion.RepairShop.Identity.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Identity/Kamirion.RepairShop.Identity.Infrastructure/"]

# Inventory
COPY ["src/Modules/Kamirion.RepairShop.Inventory/Kamirion.RepairShop.Inventory.Contracts/Kamirion.RepairShop.Inventory.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Inventory/Kamirion.RepairShop.Inventory.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Inventory/Kamirion.RepairShop.Inventory.Domain/Kamirion.RepairShop.Inventory.Domain.csproj", "src/Modules/Kamirion.RepairShop.Inventory/Kamirion.RepairShop.Inventory.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Inventory/Kamirion.RepairShop.Inventory.Application/Kamirion.RepairShop.Inventory.Application.csproj", "src/Modules/Kamirion.RepairShop.Inventory/Kamirion.RepairShop.Inventory.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Inventory/Kamirion.RepairShop.Inventory.Infrastructure/Kamirion.RepairShop.Inventory.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Inventory/Kamirion.RepairShop.Inventory.Infrastructure/"]

# Notifications
COPY ["src/Modules/Kamirion.RepairShop.Notifications/Kamirion.RepairShop.Notifications.Contracts/Kamirion.RepairShop.Notifications.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Notifications/Kamirion.RepairShop.Notifications.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Notifications/Kamirion.RepairShop.Notifications.Domain/Kamirion.RepairShop.Notifications.Domain.csproj", "src/Modules/Kamirion.RepairShop.Notifications/Kamirion.RepairShop.Notifications.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Notifications/Kamirion.RepairShop.Notifications.Application/Kamirion.RepairShop.Notifications.Application.csproj", "src/Modules/Kamirion.RepairShop.Notifications/Kamirion.RepairShop.Notifications.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Notifications/Kamirion.RepairShop.Notifications.Infrastructure/Kamirion.RepairShop.Notifications.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Notifications/Kamirion.RepairShop.Notifications.Infrastructure/"]

# Payments
COPY ["src/Modules/Kamirion.RepairShop.Payments/Kamirion.RepairShop.Payments.Contracts/Kamirion.RepairShop.Payments.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Payments/Kamirion.RepairShop.Payments.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Payments/Kamirion.RepairShop.Payments.Domain/Kamirion.RepairShop.Payments.Domain.csproj", "src/Modules/Kamirion.RepairShop.Payments/Kamirion.RepairShop.Payments.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Payments/Kamirion.RepairShop.Payments.Application/Kamirion.RepairShop.Payments.Application.csproj", "src/Modules/Kamirion.RepairShop.Payments/Kamirion.RepairShop.Payments.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Payments/Kamirion.RepairShop.Payments.Infrastructure/Kamirion.RepairShop.Payments.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Payments/Kamirion.RepairShop.Payments.Infrastructure/"]

# RepairWorkflow
COPY ["src/Modules/Kamirion.RepairShop.RepairWorkflow/Kamirion.RepairShop.RepairWorkflow.Contracts/Kamirion.RepairShop.RepairWorkflow.Contracts.csproj", "src/Modules/Kamirion.RepairShop.RepairWorkflow/Kamirion.RepairShop.RepairWorkflow.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.RepairWorkflow/Kamirion.RepairShop.RepairWorkflow.Domain/Kamirion.RepairShop.RepairWorkflow.Domain.csproj", "src/Modules/Kamirion.RepairShop.RepairWorkflow/Kamirion.RepairShop.RepairWorkflow.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.RepairWorkflow/Kamirion.RepairShop.RepairWorkflow.Application/Kamirion.RepairShop.RepairWorkflow.Application.csproj", "src/Modules/Kamirion.RepairShop.RepairWorkflow/Kamirion.RepairShop.RepairWorkflow.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.RepairWorkflow/Kamirion.RepairShop.RepairWorkflow.Infrastructure/Kamirion.RepairShop.RepairWorkflow.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.RepairWorkflow/Kamirion.RepairShop.RepairWorkflow.Infrastructure/"]

# Search
COPY ["src/Modules/Kamirion.RepairShop.Search/Kamirion.RepairShop.Search.Contracts/Kamirion.RepairShop.Search.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Search/Kamirion.RepairShop.Search.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Search/Kamirion.RepairShop.Search.Domain/Kamirion.RepairShop.Search.Domain.csproj", "src/Modules/Kamirion.RepairShop.Search/Kamirion.RepairShop.Search.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Search/Kamirion.RepairShop.Search.Application/Kamirion.RepairShop.Search.Application.csproj", "src/Modules/Kamirion.RepairShop.Search/Kamirion.RepairShop.Search.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Search/Kamirion.RepairShop.Search.Infrastructure/Kamirion.RepairShop.Search.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Search/Kamirion.RepairShop.Search.Infrastructure/"]

# Tenancy
COPY ["src/Modules/Kamirion.RepairShop.Tenancy/Kamirion.RepairShop.Tenancy.Contracts/Kamirion.RepairShop.Tenancy.Contracts.csproj", "src/Modules/Kamirion.RepairShop.Tenancy/Kamirion.RepairShop.Tenancy.Contracts/"]
COPY ["src/Modules/Kamirion.RepairShop.Tenancy/Kamirion.RepairShop.Tenancy.Domain/Kamirion.RepairShop.Tenancy.Domain.csproj", "src/Modules/Kamirion.RepairShop.Tenancy/Kamirion.RepairShop.Tenancy.Domain/"]
COPY ["src/Modules/Kamirion.RepairShop.Tenancy/Kamirion.RepairShop.Tenancy.Application/Kamirion.RepairShop.Tenancy.Application.csproj", "src/Modules/Kamirion.RepairShop.Tenancy/Kamirion.RepairShop.Tenancy.Application/"]
COPY ["src/Modules/Kamirion.RepairShop.Tenancy/Kamirion.RepairShop.Tenancy.Infrastructure/Kamirion.RepairShop.Tenancy.Infrastructure.csproj", "src/Modules/Kamirion.RepairShop.Tenancy/Kamirion.RepairShop.Tenancy.Infrastructure/"]

RUN dotnet restore "src/Kamirion.RepairShop.Web/Kamirion.RepairShop.Web.csproj"

COPY . .
WORKDIR "/src/src/Kamirion.RepairShop.Web"
RUN dotnet build "Kamirion.RepairShop.Web.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "Kamirion.RepairShop.Web.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kamirion.RepairShop.Web.dll"]
