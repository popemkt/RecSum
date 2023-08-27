## How to run
- Use an IDE of choice to run the project normally (VsCode, VisualStudio, Rider)
- Use the command line

## Considerations
- Do want to on program.cs
- Do want to implement proper error handling, which emcompasses domain error as well, all within the business logic pipeline
- Work more on the dynamic supported currencies
- Explore more summary statistics?

## Decisions
- Simple separated of concerns architecture
- This assumes the data is already partitioned by Tenant/Client, so all data ingested belongs to 1 Tenant.
- Imports with updates of existing invoices will skip the updates, as we need LastUpdatedDate property to properly handle updates

## Documentation
- The diagram
- 

## Summary
- Time spent: ~5h
- 