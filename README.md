# CommentsDemo
Assignement about Azure Table

For detailed accessability information please see the /swagger/index.html subpage.

ASP.NET Core, REST API
Azure Storage Table usage based on the Log trail pattern

## Example UseCase ##

 1. /Product/Overview

- List about the Products with names, comment amount and the id of the latest comment.

- Note: ProductName "BestProductNamedAs4000" LatestComment "2517904156255940109"
- This is the most commented product, with at least 7000 comment.

2. /Product/Comments?productname=BestProductNamedAs4000&limit=50&offset=0

- View the most recent 50 comment about the BestProductNamedAs4000 product.

3. a) Write a fresh comment to the selected product. Pass the previously noted LatestComment id.

curl -X POST "http://localhost:60613/Product/NewComment" -H  "accept: application/json" -H  "Content-Type: application/json" -d "{\"productName\":\"BestProductNamedAs4000\",\"commentContent\":\"example comment\",\"latestComment\":\"2517904156255940109\"}"

3. b) Write a not so fresh comment. Reuse the originally noted comment id. It will return as a Bad Request and detail: "The latest comment id is not valid!",

## Limitations, findings  ##
- Poor solution to identify the freshness of a new comment. Trust in user input.
- Self hosted, missed the Azure deployment because of authorization troubles.
- Partition and Row keys are not sanitized for invalid chars.
- Hot partition problem can occure
- Lack of scalability aspects
- Wrong dll structure, web publish not possible
