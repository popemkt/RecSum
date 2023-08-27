## Considerations
- Add error handling
- loggings
- Merge Application and Domain?
- remove design package in api
- CurrencyCode enum?
- Config on program.cs
- 
## Decisions
- Simple separated of concerns architecture
- This assumes the data is already partitioned by Tenant/Client, so all data ingested belongs to 1 Tenant.
- Imports with updates of existing invoices will skip the updates, as we need LastUpdatedDate property to properly handle updates