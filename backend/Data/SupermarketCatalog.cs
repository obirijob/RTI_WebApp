namespace Inventory.Api.Data;

internal sealed record DepartmentSeed(
    string Name,
    string Code,
    decimal BasePrice,
    decimal PriceStep,
    string[] Products);

internal static class SupermarketCatalog
{
    public static readonly string[] PackVariants =
    [
        "",
        "Value Pack",
        "Family Pack",
        "Economy Pack",
        "Twin Pack",
        "Bulk Pack",
        "Premium Pack",
        "Mini Pack",
        "Party Pack",
        "Wholesale Pack"
    ];

    public static readonly DepartmentSeed[] Departments =
    [
        new("Fresh Produce", "FPR", 50, 20,
        [
            "Cavendish Bananas 1kg", "Hass Avocados 4 Pack", "Sukuma Wiki 500g",
            "Roma Tomatoes 1kg", "Red Onions 1kg", "Irish Potatoes 2kg",
            "Royal Gala Apples 1kg", "Kent Mangoes 1kg", "Carrots 1kg",
            "Sweet Potatoes 2kg"
        ]),
        new("Bakery", "BAK", 60, 25,
        [
            "Broadways White Bread 600g", "Super Loaf Brown Bread 600g",
            "Festive Wholemeal Bread 600g", "Supa Loaf Sandwich Bread 800g",
            "Milk Bread 400g", "Fresh Croissants 6 Pack", "Cinnamon Rolls 4 Pack",
            "Mandazi 6 Pack", "Burger Buns 6 Pack", "French Baguette"
        ]),
        new("Dairy & Eggs", "DAI", 75, 35,
        [
            "Brookside Whole Milk 1L", "KCC Fresh Milk 500ml", "Fresha Long Life Milk 1L",
            "Daima Strawberry Yoghurt 450g", "Bio Plain Yoghurt 450g",
            "Ilara Vanilla Yoghurt 250ml", "Blue Band Margarine 500g",
            "Brookside Salted Butter 500g", "Farm Fresh Eggs 12 Pack", "Brown Eggs 6 Pack"
        ]),
        new("Meat & Poultry", "MTP", 250, 100,
        [
            "Kenchic Whole Chicken 1.2kg", "Farmers Choice Beef Sausages 500g",
            "Prime Beef Mince 500g", "Goat Stew Cubes 500g", "Beef Sirloin Steak 500g",
            "Kenchic Chicken Breast 500g", "Kenchic Chicken Wings 1kg",
            "Pork Chops 500g", "Lamb Shoulder 500g", "Smoked Beef Bacon 250g"
        ]),
        new("Seafood", "SEA", 280, 120,
        [
            "Lake Victoria Tilapia Whole 1kg", "Nile Perch Fillet 500g",
            "Fresh Omena 500g", "King Prawns 500g", "Tuna Steak 500g",
            "Norwegian Salmon Fillet 500g", "Calamari Rings 500g", "Smoked Sailfish 250g",
            "Mud Crab 1kg", "Breaded Fish Fillets 500g"
        ]),
        new("Deli & Prepared Foods", "DEL", 80, 60,
        [
            "Beef Samosas 4 Pack", "Chicken Samosas 4 Pack", "Vegetable Spring Rolls 6 Pack",
            "Ready-Made Pilau 500g", "Ready-Made Beef Biryani 500g", "Chapati 6 Pack",
            "Rotisserie Chicken Whole", "Chicken Kebabs 4 Pack", "Fresh Kachumbari 300g",
            "Sausage Rolls 4 Pack"
        ]),
        new("Frozen Foods", "FRZ", 180, 70,
        [
            "McCain French Fries 1kg", "Kenchic Chicken Nuggets 500g",
            "Farmers Choice Beef Burgers 600g", "Frozen Garden Peas 1kg",
            "Frozen Mixed Vegetables 1kg", "Frozen Tilapia Fillets 500g",
            "Vanilla Ice Cream 1L", "Chocolate Ice Cream 1L", "Frozen Samosas 12 Pack",
            "Frozen Pizza Margherita 400g"
        ]),
        new("Pantry Staples", "PAN", 100, 50,
        [
            "Unga Dola Maize Meal 2kg", "Pembe Maize Meal 2kg", "Jogoo Maize Meal 2kg",
            "Soko Home Baking Flour 2kg", "Mwea Pishori Rice 2kg", "Daawat Basmati Rice 2kg",
            "Kenylon Green Grams 1kg", "Kamande Lentils 1kg", "Mumias Sugar 2kg",
            "Kensalt Table Salt 1kg"
        ]),
        new("Breakfast & Cereals", "BRC", 180, 70,
        [
            "Weetabix Original 450g", "Kellogg's Corn Flakes 500g", "Quaker Oats 500g",
            "Famila Pure Wimbi Porridge 1kg", "Nutripro Millet Porridge 1kg",
            "Morning Harvest Granola 500g", "Nestle Milo Cereal 330g",
            "Kellogg's Coco Pops 375g", "Weetabix Oatibix 550g", "Tropical Muesli 500g"
        ]),
        new("Snacks & Confectionery", "SNK", 30, 25,
        [
            "Tropical Heat Salted Crisps 150g", "Tropical Heat Chevda 150g",
            "Cadbury Dairy Milk 80g", "Nestle KitKat 4 Finger", "Manji Digestive Biscuits 200g",
            "Britannia Nice Biscuits 200g", "Blue Band Peanut Butter 400g",
            "Kenya Nut Cashews 150g", "Patco Tropical Sweets 100g", "Crispy Roasted Peanuts 200g"
        ]),
        new("Soft Drinks & Juices", "BEV", 50, 40,
        [
            "Coca-Cola Original 2L", "Fanta Orange 2L", "Sprite 2L", "Stoney Tangawizi 2L",
            "Keringet Mineral Water 1L", "Dasani Drinking Water 1.5L",
            "Minute Maid Mango Juice 1L", "Afia Mixed Fruit Juice 1L",
            "Del Monte Pineapple Juice 1L", "Red Bull Energy Drink 250ml"
        ]),
        new("Tea & Coffee", "TCF", 120, 80,
        [
            "Kericho Gold Tea Bags 100 Pack", "Ketepa Pride Tea Leaves 500g",
            "Fahari ya Kenya Tea 500g", "Baraka Chai 500g", "Dormans House Blend Coffee 250g",
            "Java House Ground Coffee 375g", "Nescafe Classic Coffee 200g",
            "Dormans Instant Coffee 100g", "Melvins Masala Tea 100g", "Sasini Green Tea 25 Pack"
        ]),
        new("Canned & Jarred Foods", "CAN", 100, 50,
        [
            "Heinz Baked Beans 415g", "Peptang Tomato Paste 400g", "Njoro Canned Green Peas 400g",
            "American Garden Sweet Corn 400g", "Sunshine Tuna Chunks 185g",
            "Zesta Mixed Fruit Jam 500g", "Blue Band Peanut Butter 800g",
            "Kentaste Coconut Milk 400ml", "Canned Chickpeas 400g", "Canned Button Mushrooms 400g"
        ]),
        new("Condiments & Sauces", "CON", 80, 50,
        [
            "Zesta Tomato Ketchup 500g", "Peptang Chilli Sauce 400g", "Royco Mchuzi Mix 200g",
            "Knorr Beef Cubes 100g", "Heinz Mayonnaise 500ml", "American Garden Soy Sauce 500ml",
            "Tabasco Pepper Sauce 60ml", "Kentaste Coconut Sauce 300ml",
            "Aromat Seasoning 200g", "French's Yellow Mustard 226g"
        ]),
        new("Household Cleaning", "CLN", 100, 70,
        [
            "Omo Hand Washing Powder 1kg", "Ariel Washing Powder 1kg",
            "Sunlight Dishwashing Liquid 750ml", "Vim Scouring Powder 500g",
            "Jik Regular Bleach 1L", "Dettol Disinfectant 1L", "Harpic Toilet Cleaner 500ml",
            "Morning Fresh Dishwashing Liquid 750ml", "Sta-soft Fabric Softener 2L",
            "Zenta Multipurpose Cleaner 1L"
        ]),
        new("Paper & Disposables", "PAP", 80, 50,
        [
            "Hanan Toilet Tissue 10 Pack", "Velvex Toilet Tissue 10 Pack",
            "Kleenex Facial Tissues 100 Pack", "Rosy Kitchen Towels 2 Pack",
            "White Paper Serviettes 100 Pack", "Aluminium Foil 30m", "Cling Film 30m",
            "Heavy Duty Bin Bags 20 Pack", "Disposable Paper Cups 25 Pack",
            "Disposable Dinner Plates 25 Pack"
        ]),
        new("Personal Care", "PER", 80, 70,
        [
            "Geisha Coconut Bath Soap 175g", "Dettol Original Soap 175g",
            "Colgate Herbal Toothpaste 140g", "Sensodyne Fresh Mint Toothpaste 100ml",
            "Vaseline Cocoa Radiant Lotion 400ml", "Nivea Men Body Lotion 400ml",
            "Always Maxi Thick Pads 16 Pack", "Kotex Ultra Thin Pads 16 Pack",
            "Head & Shoulders Shampoo 400ml", "Nice & Lovely Hair Food 250g"
        ]),
        new("Baby Care", "BAB", 150, 150,
        [
            "Pampers Baby-Dry Size 3 44 Pack", "Huggies Dry Comfort Size 4 36 Pack",
            "Molfix Pants Size 5 30 Pack", "Nestle Cerelac Wheat 500g",
            "NAN Optipro Infant Formula 400g", "Johnson's Baby Lotion 500ml",
            "Cussons Baby Oil 200ml", "Pampers Sensitive Wipes 56 Pack",
            "Huggies Pure Wipes 56 Pack", "Baby Love Cotton Buds 100 Pack"
        ]),
        new("Health & Wellness", "HEA", 50, 80,
        [
            "Panadol Extra 20 Tablets", "ENO Fruit Salt 100g", "ORS Rehydration Salts 10 Pack",
            "Dettol Antiseptic Liquid 500ml", "Disposable Face Masks 50 Pack",
            "Surgical Cotton Wool 100g", "Elastic Bandage 10cm", "Lifebuoy Hand Sanitizer 500ml",
            "Centrum Multivitamins 30 Pack", "Dextrose Glucose Powder 500g"
        ]),
        new("Pet Supplies", "PET", 150, 100,
        [
            "Pedigree Adult Dog Food 3kg", "Whiskas Tuna Cat Food 1.2kg",
            "Bonnie Beef Dog Food 2kg", "Cat Litter Lavender 5kg", "Dog Treat Biscuits 500g",
            "Pet Shampoo 500ml", "Wild Bird Seed 1kg", "Tropical Fish Flakes 100g",
            "Rawhide Dog Chews 10 Pack", "Cat Treats Chicken 200g"
        ])
    ];
}
