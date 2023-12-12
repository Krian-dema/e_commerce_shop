<script setup>
import SlIconAlert from '@/components/common/icons/SlIconAlert.vue';

defineProps({
  label: {
    type: String,
    required: true,
  },
});
</script>
<style>
.SlIconAlertContainer a{
  color: darkred;
}
</style>
<template>
  <div class="flex items-center p-4 text-sm rounded-lg bg-red-50 text-red-600" role="alert">
    <SlIconAlert class="flex-shrink-0 inline w-4 h-4 mr-3" />
    <span v-html="label" class="SlIconAlertContainer" >
    </span>
  </div>
</template>
