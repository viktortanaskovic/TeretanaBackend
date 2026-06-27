using Microsoft.EntityFrameworkCore;
using TeretanaBackend.Data;
using TeretanaBackend.Dto;
using TeretanaBackend.Models;
using TeretanaBackend.Services.Interfaces;

namespace TeretanaBackend.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext dbContext;

        public CartService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Cart> AddCartAsync(Cart cart, string userName)
        {
            var userExists = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == cart.UserId);

            if (userExists is null) throw new Exception("User does not exists");

            var cartExists = await dbContext.Carts.FirstOrDefaultAsync(c => c.CartId == cart.CartId && c.UserId == cart.UserId);

            if (cartExists is not null) throw new Exception("This Cart already exists");

            var newCart= new Cart()
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userName
            };

            await dbContext.Carts.AddAsync(newCart);
            await dbContext.SaveChangesAsync();

            return newCart;
        }

        public async Task<string?> DeleteCart(Cart cart)
        {
            var cartExists = await dbContext.Carts.FirstOrDefaultAsync(c=>c.CartId == cart.CartId && c.UserId==cart.UserId);

            if (cartExists is null) return null;

            dbContext.Carts.Remove(cart);
            await dbContext.SaveChangesAsync();

            return "Given Cart has been deleted successfully";
        }

        public async Task<List<Cart>> GetAllCartsAsync()
        {
            return await dbContext.Carts.ToListAsync();
        }

        public async Task<Cart> GetCartAsync(Cart cart)
        {
            var cartExists = await dbContext.Carts.FirstOrDefaultAsync(c=>c.CartId==cart.CartId && c.UserId == cart.UserId);

            if (cartExists is null) throw new Exception("Given Cart does not exists");

            return cartExists;
        }

        public async Task<Cart> UpdateCartAsync(Cart cart, string userName)
        {
            var cartExists = await dbContext.Carts.FirstOrDefaultAsync(c => c.CartId == cart.CartId);

            if (cartExists is null) throw new Exception("Given Cart does not exists");

            cartExists.UserId = cart.UserId;
            await dbContext.SaveChangesAsync();

            return cartExists;
        }

        public async Task<CartItemsResponse> AddCartWIthItemsAsync(CartItemsRequest cartItemsRequest)
        {
            var response = new CartItemsResponse();

            var cartExists = await dbContext.Carts.FirstOrDefaultAsync(c => c.UserId == cartItemsRequest.Cart.UserId && c.CartId == cartItemsRequest.Cart.CartId);

            if (cartExists is not null) throw new Exception("Given Cart already exists");

            var newCart = new Cart() { 
                UserId=cartItemsRequest.Cart.UserId,
                CreatedAt=DateTime.Now,
                CreatedBy=cartItemsRequest.UserName
            };
            await dbContext.Carts.AddAsync(newCart);
            await dbContext.SaveChangesAsync();

            response.Cart=newCart;

            var listItems = new List<CartItem>();
            foreach (var item in cartItemsRequest.CartItems)
            {
                var productExists = await dbContext.Products.FirstOrDefaultAsync(p=>p.ProductId==item.ProductId);
                if (productExists is null) throw new Exception("Product does not exists");
                if (productExists.StockQuantity < item.Quantity) throw new Exception("Not enough in Stock Quantity for this Cart");

                var newItem = new CartItem() { 
                    Quantity=item.Quantity,
                    ProductId=item.ProductId,
                    CartId=newCart.CartId,
                    CreatedAt = DateTime.Now,
                    CreatedBy=cartItemsRequest.UserName
                };
                listItems.Add(newItem);
            }
            await dbContext.CartItems.AddRangeAsync(listItems);
            await dbContext.SaveChangesAsync();

            response.CartItems = listItems;

            return response;
        }

        public async Task<CartItemsResponse> UpdateCartWithItemsAsync(CartItemsRequest cartItemsRequest)
        {
            var cartExists = await dbContext.Carts.FirstOrDefaultAsync(c=>c.CartId==cartItemsRequest.Cart.CartId && c.UserId==cartItemsRequest.Cart.UserId);

            if (cartExists is null) throw new Exception("Given Cart does not exists");

            var currentCartItems = await dbContext.CartItems.Where(x => x.CartId == cartExists.CartId).ToListAsync();
            var newItemList= new List<CartItem>();
            var removeItemList = new List<CartItem>();
            foreach (var item in currentCartItems)
            {
                var exists = cartItemsRequest.CartItems.FirstOrDefault(x=>x.CartId==item.CartId && x.ProductId==item.ProductId);
                if(exists is not null) continue;
                removeItemList.Add(item);
            }
            foreach (var item in cartItemsRequest.CartItems)
            {
                //var cartItem = await dbContext.CartItems.FirstOrDefaultAsync(ci=>ci.CartId==item.CartId && ci.ProductId==item.ProductId && ci.CartItemId==item.CartItemId);
                var cartItem = currentCartItems.FirstOrDefault(x=>x.ProductId==item.ProductId);

                if (item.Quantity <= 0) item.Quantity = 1;

                if (cartItem is null)
                {
                    var newItem = new CartItem()
                    {
                        CartId= cartExists.CartId,
                        ProductId= item.ProductId,
                        Quantity= item.Quantity,
                        CreatedAt= DateTime.UtcNow,
                        CreatedBy= cartItemsRequest.UserName
                    };
                    newItemList.Add(newItem);
                }
                else if (cartItem is not null)
                {
                    cartItem.Quantity = item.Quantity;
                }
            }
            if (newItemList.Count > 0)
            {
                await dbContext.CartItems.AddRangeAsync(newItemList);
            }
            if (removeItemList.Count > 0)
            {
                dbContext.CartItems.RemoveRange(removeItemList);
            }
            await dbContext.SaveChangesAsync();

            var listItems = await dbContext.CartItems.Where(ci => ci.CartId == cartExists.CartId).ToListAsync();
            var response = new CartItemsResponse() { Cart = cartExists, CartItems=listItems};

            return response;
        }

        public async Task<CartItemsResponse> GetCartWithItemsAsync(Cart cart)
        {
            var cartExists = await dbContext.Carts.FirstOrDefaultAsync(c=>c.CartId==cart.CartId && c.UserId==cart.UserId);

            if (cartExists is null) throw new Exception("Given Cart does not exists");

            var listItems = await dbContext.CartItems.Where(li => li.CartId == cartExists.CartId).ToListAsync();

            if (listItems is null) throw new Exception("For given Cart there is no Cart Items");

            var response = new CartItemsResponse()
            {
                Cart=cartExists,
                CartItems=listItems
            };

            return response;
        }
    }
}
