using Godot;
using System;

public class Inventory {

	private ItemData storedItem;

	public ItemData GetStoredItem() {
		return this.storedItem;
	}

	public void ClearInventory() {
		this.storedItem = null;
	}

	public ItemData StoreItem(ItemData newItem) {
		if (this.storedItem != null) {
			ItemData heldItem = this.storedItem;
			this.storedItem = newItem;
			return heldItem;
		} else {
			this.storedItem = newItem;
			return null;
		}
	}

}
