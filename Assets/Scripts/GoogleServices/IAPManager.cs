using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener {

    [SerializeField] Button[] purchaseButtons;
    IStoreController controller;
    IExtensionProvider extensions;

    public static string ID_COINS_SMALL = "net.maarti.magnetic_frog.coins_small";
    public static string ID_COINS_MEDIUM = "500_coins";
    public static string ID_COINS_HIGH = "net.maarti.magnetic_frog.coins_high";

    void Start() {
        if (controller == null) {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing() {
        if (IsInitialized()) {
            return;
        }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(ID_COINS_MEDIUM, ProductType.Consumable, new IDs
        {
            {ID_COINS_MEDIUM, GooglePlay.Name},
        });
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized() {
        // Only say we are initialized if both the Purchasing references are set.        
        return controller != null && extensions != null;
    }

    private void BuyProductID(string productId) {
        if (IsInitialized()) {
            // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
            Product product = controller.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase) {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                Debug.Log(product.metadata.localizedPriceString);
                controller.InitiatePurchase(product);
            }
            else {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void BuyCoinsMedium() {
        BuyProductID(ID_COINS_MEDIUM);
    }

    void RefreshButtons() {
        if (purchaseButtons != null) {
            bool enable = IsInitialized();
            foreach (Button button in purchaseButtons) {
                button.interactable = enable;
#if UNITY_EDITOR
                button.interactable = true;
#endif
            }
        }
    }

    // IStoreListener
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        Debug.Log("IAP init");
        this.controller = controller;
        this.extensions = extensions;
        RefreshButtons();
    }

    public void OnInitializeFailed(InitializationFailureReason error) {
        Debug.Log("IAP init failed: " + error.ToString());
        RefreshButtons();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e) {
        Debug.Log("IAP purchase completed: " + e);
        if (String.Equals(e.purchasedProduct.definition.id, ID_COINS_MEDIUM, StringComparison.Ordinal)) {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", e.purchasedProduct.definition.id));
            // The consumable item has been successfully purchased, add 500 coins to the player's in-game score.
            ApplicationController.ac.UpdateCoins(500);
        }
        else {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", e.purchasedProduct.definition.id));
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p) {
        Debug.Log("IAP purchase failed: " + p.ToString());
    }


}
